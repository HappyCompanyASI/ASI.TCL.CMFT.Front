using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.SYS.Datas;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;
using ASI.TCL.CMFT.WPF.Module.SYS.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSRadioGroupSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSRadioGroupSettingsViewModel()
        {
        }
        public SYSRadioGroupSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            
            _dialogService = dialogService;
            
            _eventAggregator = eventAggregator;

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        private List<RadioGroupDto> _originalSYSRadioGroups;

        // 單一訊息控制項
        public bool IsItemOperation => SelectedGroup != null;

        private RadioGroupDto _selectedGroup;
        public RadioGroupDto SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                RaisePropertyChanged(nameof(IsItemOperation));
                RefreshRadioViews();
            }
        }

        // 無線電群組資料
        private ObservableCollection<RadioGroupDto> _sysRadioGroups = new();
        public ObservableCollection<RadioGroupDto> SYSRadioGroups
        {
            get => _sysRadioGroups;
            set => SetProperty(ref _sysRadioGroups, value);
        }

        private ObservableCollection<RadioDto> _allsysRadios = new();
        public ObservableCollection<RadioDto> AllsysRadios
        {
            get => _allsysRadios;
            set => SetProperty(ref _allsysRadios, value);
        }

        public ICollectionView UnselectedRadiosView { get; private set; }
        public ICollectionView SelectedRadiosView { get; private set; }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addGroupCommand;
        public DelegateCommand AddGroupCommand => _addGroupCommand ??= new DelegateCommand(ExcuteAddGroupCommand);
        private void ExcuteAddGroupCommand()
        {
            _dialogService.ShowDialogHost(nameof(SYSGroupSettingsDialogView), null, result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey("GroupName"))
                {
                    string groupName = result.Parameters.GetValue<string>("GroupName");

                    string guid = Guid.NewGuid().ToString();

                    //畫面
                    var newGroup = new RadioGroupDto
                    {
                        Id = guid,
                        GroupName = groupName,
                        Radios = new List<RadioDto>()
                    };

                    //資料庫

                    SYSRadioGroups.Add(newGroup);
                    UpdateSnapshot();
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _deleteGroupCommand;
        public DelegateCommand DeleteGroupCommand => _deleteGroupCommand ??= new DelegateCommand(ExcuteDeleteGroupCommand);
        private void ExcuteDeleteGroupCommand()
        {
            var groupToRemove = SelectedGroup;

            if (SelectedGroup == null || SelectedGroup.GroupName == "全部無線電")
                return;

            _dialogService.IsDelete($"確定要刪除\"{groupToRemove.GroupName}\"這個群組嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //資料庫

                    // 從群組集合中移除該群組
                    SYSRadioGroups.Remove(groupToRemove);

                    // 更新快照
                    UpdateSnapshot();

                    // 刷新 ListView 顯示
                    RefreshRadioViews();
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedGroup;

            //資料庫

            UpdateSnapshot();

            IsEditMode = false;
        }

        private DelegateCommand _editCommand;
        public DelegateCommand EditCommand => _editCommand ??= new DelegateCommand(ExcuteEditCommand);
        private void ExcuteEditCommand()
        {
            IsEditMode = true;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {
            if (!HasChanges())
            {
                IsEditMode = false;
                return;
            }
            SYSRadioGroups = new ObservableCollection<RadioGroupDto>(
                _originalSYSRadioGroups.Select(g => new RadioGroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    Radios = g.Radios == null ? null : g.Radios.Select(r => new RadioDto
                    {
                        RadioNumber = r.RadioNumber
                    }).ToList()
                })
            );
            RaisePropertyChanged(nameof(SYSRadioGroups));
            SelectedGroup = null;
            IsEditMode = false;
        }

        private DelegateCommand<RadioDto> _selectRadioCommand;
        public DelegateCommand<RadioDto> SelectRadioCommand => _selectRadioCommand ??= new DelegateCommand<RadioDto>(ExecuteSelectRadioCommand);
        private void ExecuteSelectRadioCommand(RadioDto radio)
        {
            if (radio == null || SelectedGroup == null) return;

            // 用 RadioNumber 找到要移除的物件
            var exist = SelectedGroup.Radios?.FirstOrDefault(r => r.RadioNumber.Equals(radio.RadioNumber));
            if (exist != null)
                SelectedGroup.Radios.Remove(exist);
            else
            {
                if (SelectedGroup.Radios == null)
                    SelectedGroup.Radios = new List<RadioDto>();
                SelectedGroup.Radios.Add(radio);
            }

            RefreshRadioViews();
        }

        // 編輯模式時選項變更或離開時視作取消編輯
        private DelegateCommand _cancelEditOnSelectionChangedCommand;
        public DelegateCommand CancelEditOnSelectionChangedCommand =>
            _cancelEditOnSelectionChangedCommand ??= new DelegateCommand(ExecuteCancelEditOnSelectionChangedCommand);
        private void ExecuteCancelEditOnSelectionChangedCommand()
        {
            if (IsEditMode)
            {
                CancelCommand.Execute();
            }
        }
        #endregion

        #region Private Methods
        private void RefreshRadioViews()
        {
            UnselectedRadiosView?.Refresh();
            SelectedRadiosView?.Refresh();
        }
        private bool Unselected_Filter(object item)
        {
            if (item is not RadioDto radio || SelectedGroup == null)
                return false;

            if (SelectedGroup.Radios == null)
                return true;

            // 如果這個無線電(RadioNumber)不在SelectedGroup.Radios裡，則顯示
            return !SelectedGroup.Radios.Any(r => r.RadioNumber.Equals(radio.RadioNumber));
        }
        private bool Selected_Filter(object item)
        {
            if (item is RadioDto r && SelectedGroup != null)
            {
                // 若該無線電在 Radios，就顯示在右邊
                return SelectedGroup.Radios != null && SelectedGroup.Radios.Any(rd => rd.RadioNumber.Equals(r.RadioNumber));
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalSYSRadioGroups == null || SYSRadioGroups == null ||
                _originalSYSRadioGroups.Count != SYSRadioGroups.Count ||
                _originalSYSRadioGroups.Zip(SYSRadioGroups, (orig, curr) =>
                    orig.Id != curr.Id ||
                    orig.GroupName != curr.GroupName ||
                    (orig.Radios == null && curr.Radios != null) ||
                    (orig.Radios != null && curr.Radios == null) ||
                    (orig.Radios != null && curr.Radios != null &&
                        (orig.Radios.Count != curr.Radios.Count ||
                         orig.Radios.Zip(curr.Radios, (o, c) => !o.RadioNumber.Equals(c.RadioNumber)).Any(x => x))
                    )
                ).Any(x => x))
                return true;
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalSYSRadioGroups = SYSRadioGroups
                .Select(g => new RadioGroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    Radios = g.Radios == null ? null : g.Radios.Select(r => new RadioDto
                    {
                        RadioNumber = r.RadioNumber
                    }).ToList()
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            // 更新 ViewModel 屬性
            SYSRadioGroups = new ObservableCollection<RadioGroupDto>(DesignTimeDatas.RadioGroups);
            AllsysRadios = new ObservableCollection<RadioDto>(DesignTimeDatas.Radios);

            // 主動指定選擇第一筆群組
            SelectedGroup = SYSRadioGroups.FirstOrDefault();

            // 設定 CollectionView 筆刷
            var cvsUnselected = new CollectionViewSource { Source = AllsysRadios };
            UnselectedRadiosView = cvsUnselected.View;
            UnselectedRadiosView.Filter = Unselected_Filter;

            var cvsSelected = new CollectionViewSource { Source = AllsysRadios };
            SelectedRadiosView = cvsSelected.View;
            SelectedRadiosView.Filter = Selected_Filter;

            // 通知畫面更新

            //快照初始化
            UpdateSnapshot();
        }
        #endregion
    }
}
