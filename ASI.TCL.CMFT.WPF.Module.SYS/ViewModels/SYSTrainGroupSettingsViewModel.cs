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
    public class SYSTrainGroupSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSTrainGroupSettingsViewModel()
        {
        }
        public SYSTrainGroupSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<SYSTrainGroupDto> _originalSYSTrainGroups;

        // 單一訊息控制項
        public bool IsItemOperation => SelectedGroup != null;

        private SYSTrainGroupDto _selectedGroup;
        public SYSTrainGroupDto SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                RaisePropertyChanged(nameof(IsItemOperation));
                RefreshTrainViews();
            }
        }

        // 列車群組資料
        private ObservableCollection<SYSTrainGroupDto> _sysTrainGroups = new();
        public ObservableCollection<SYSTrainGroupDto> SYSTrainGroups
        {
            get => _sysTrainGroups;
            set => SetProperty(ref _sysTrainGroups, value);
        }

        private ObservableCollection<SYSTrainDto> _allsysTrains = new();
        public ObservableCollection<SYSTrainDto> AllsysTrains
        {
            get => _allsysTrains;
            set => SetProperty(ref _allsysTrains, value);
        }

        public ICollectionView UnselectedTrainsView { get; private set; }
        public ICollectionView SelectedTrainsView { get; private set; }
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

                    Guid guid = Guid.NewGuid();

                    //畫面
                    var newGroup = new SYSTrainGroupDto
                    {
                        Id = guid,
                        GroupName = groupName,
                        Trains = new List<SYSTrainDto>()
                    };

                    //資料庫

                    SYSTrainGroups.Add(newGroup);
                    UpdateSnapshot();
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _deleteGroupCommand;
        public DelegateCommand DeleteGroupCommand => _deleteGroupCommand ??= new DelegateCommand(ExcuteDeleteGroupCommand);
        private void ExcuteDeleteGroupCommand()
        {
            var groupToRemove = SelectedGroup;

            if (SelectedGroup == null || SelectedGroup.GroupName == "全部")
                return;

            _dialogService.IsDelete($"確定要刪除\"{groupToRemove.GroupName}\"這個群組嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //資料庫

                    // 從群組集合中移除該群組
                    SYSTrainGroups.Remove(groupToRemove);

                    // 更新快照
                    UpdateSnapshot();

                    // 刷新 ListView 顯示
                    RefreshTrainViews();
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
            SYSTrainGroups = new ObservableCollection<SYSTrainGroupDto>(
                _originalSYSTrainGroups.Select(g => new SYSTrainGroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    Trains = g.Trains == null ? null : g.Trains.Select(t => new SYSTrainDto
                    {
                        Id = t.Id,
                        TrainNumber = t.TrainNumber
                    }).ToList()
                })
            );
            RaisePropertyChanged(nameof(SYSTrainGroups));
            SelectedGroup = null;
            IsEditMode = false;
        }

        private DelegateCommand<SYSTrainDto> _selectTrainCommand;
        public DelegateCommand<SYSTrainDto> SelectTrainCommand => _selectTrainCommand ??= new DelegateCommand<SYSTrainDto>(ExecuteSelectTrainCommand);
        private void ExecuteSelectTrainCommand(SYSTrainDto train)
        {
            if (train == null || SelectedGroup == null) return;

            // 用 TrainNumber 找到要移除的物件
            var exist = SelectedGroup.Trains.FirstOrDefault(t => t.TrainNumber == train.TrainNumber);
            if (exist != null)
                SelectedGroup.Trains.Remove(exist);
            else
                SelectedGroup.Trains.Add(train);

            RefreshTrainViews();
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
        private void RefreshTrainViews()
        {
            UnselectedTrainsView?.Refresh();
            SelectedTrainsView?.Refresh();
        }
        private bool Unselected_Filter(object item)
        {
            if (item is not SYSTrainDto train || SelectedGroup == null)
                return false;

            // 如果SelectedGroup.Trains為null，則全部顯示
            if (SelectedGroup.Trains == null)
                return true;

            // 如果這個車站(TrainNumber)不在SelectedGroup.Trains裡，則顯示
            return !SelectedGroup.Trains.Any(t => t.TrainNumber == train.TrainNumber);
        }
        private bool Selected_Filter(object item)
        {
            if (item is SYSTrainDto s && SelectedGroup != null)
            {
                // 若該站在 Targets，就顯示在右邊
                return SelectedGroup.Trains.Any(t => t.TrainNumber == s.TrainNumber);
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalSYSTrainGroups == null || SYSTrainGroups == null ||
                _originalSYSTrainGroups.Count != SYSTrainGroups.Count ||
                _originalSYSTrainGroups.Zip(SYSTrainGroups, (orig, curr) =>
                    orig.Id != curr.Id ||
                    orig.GroupName != curr.GroupName ||
                    (orig.Trains == null && curr.Trains != null) ||
                    (orig.Trains != null && curr.Trains == null) ||
                    (orig.Trains != null && curr.Trains != null &&
                        (orig.Trains.Count != curr.Trains.Count ||
                         orig.Trains.Zip(curr.Trains, (o, c) => o.TrainNumber != c.TrainNumber).Any(x => x))
                    )
                ).Any(x => x))
                return true;
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalSYSTrainGroups = SYSTrainGroups
                .Select(g => new SYSTrainGroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    Trains = g.Trains == null ? null : g.Trains.Select(t => new SYSTrainDto
                    {
                        Id = t.Id,
                        TrainNumber = t.TrainNumber
                    }).ToList()
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            // 更新 ViewModel 屬性
            SYSTrainGroups = new ObservableCollection<SYSTrainGroupDto>(DesignTimeDatas.SYSTrainGroups);
            AllsysTrains = new ObservableCollection<SYSTrainDto>(DesignTimeDatas.SYSTrains);

            // 主動指定選擇第一筆群組
            SelectedGroup = SYSTrainGroups.FirstOrDefault();

            // 設定 CollectionView 筆刷
            var cvsUnselected = new CollectionViewSource { Source = AllsysTrains };
            UnselectedTrainsView = cvsUnselected.View;
            UnselectedTrainsView.Filter = Unselected_Filter;

            var cvsSelected = new CollectionViewSource { Source = AllsysTrains };
            SelectedTrainsView = cvsSelected.View;
            SelectedTrainsView.Filter = Selected_Filter;

            // 通知畫面更新

            //快照初始化
            UpdateSnapshot();
        }
        #endregion
    }
}
