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
    public class SYSStationGroupSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSStationGroupSettingsViewModel()
        {
        }
        public SYSStationGroupSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<SYSStationGroupDto> _originalSYSStationGroups;

        // 單一訊息控制項
        public bool IsItemOperation => SelectedGroup != null;

        private SYSStationGroupDto _selectedGroup;
        public SYSStationGroupDto SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                RaisePropertyChanged(nameof(IsItemOperation));
                RefreshStationViews();
            }
        }

        // 車站群組資料
        private ObservableCollection<SYSStationGroupDto> _sysStationGroups = new();
        public ObservableCollection<SYSStationGroupDto> SYSStationGroups
        {
            get => _sysStationGroups;
            set => SetProperty(ref _sysStationGroups, value);
        }

        private ObservableCollection<SYSStationDto> _allsysStations = new();
        public ObservableCollection<SYSStationDto> AllsysStations
        {
            get => _allsysStations;
            set => SetProperty(ref _allsysStations, value);
        }

        public ICollectionView UnselectedStationsView { get; private set; }
        public ICollectionView SelectedStationsView { get; private set; }
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
                    var newGroup = new SYSStationGroupDto
                    {
                        Id = guid,
                        GroupName = groupName,
                        Stations = new List<SYSStationDto>()
                    };

                    //資料庫

                    SYSStationGroups.Add(newGroup);
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
                    SYSStationGroups.Remove(groupToRemove);

                    // 更新快照
                    UpdateSnapshot();

                    // 刷新 ListView 顯示
                    RefreshStationViews();
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
            SYSStationGroups = new ObservableCollection<SYSStationGroupDto>(
                _originalSYSStationGroups.Select(g => new SYSStationGroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    Stations = g.Stations == null ? null : g.Stations.Select(s => new SYSStationDto
                    {
                        StationID = s.StationID,
                        StationName = s.StationName
                    }).ToList()
                })
            );
            RaisePropertyChanged(nameof(SYSStationGroups));
            SelectedGroup = null;
            IsEditMode = false;
        }

        private DelegateCommand<SYSStationDto> _selectStationCommand;
        public DelegateCommand<SYSStationDto> SelectStationCommand => _selectStationCommand ??= new DelegateCommand<SYSStationDto>(ExecuteSelectStationCommand);
        private void ExecuteSelectStationCommand(SYSStationDto station)
        {
            if (station == null || SelectedGroup == null) return;

            // 用 StationID 找到要移除的物件
            var exist = SelectedGroup.Stations?.FirstOrDefault(s => s.StationID.Equals(station.StationID));
            if (exist != null)
                SelectedGroup.Stations.Remove(exist);
            else
            {
                if (SelectedGroup.Stations == null)
                    SelectedGroup.Stations = new List<SYSStationDto>();
                SelectedGroup.Stations.Add(station);
            }

            RefreshStationViews();
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
        private void RefreshStationViews()
        {
            UnselectedStationsView?.Refresh();
            SelectedStationsView?.Refresh();
        }
        private bool Unselected_Filter(object item)
        {
            if (item is not SYSStationDto station || SelectedGroup == null)
                return false;

            if (SelectedGroup.Stations == null)
                return true;

            // 如果這個車站(StationID)不在SelectedGroup.Stations裡，則顯示
            return !SelectedGroup.Stations.Any(s => s.StationID.Equals(station.StationID));
        }
        private bool Selected_Filter(object item)
        {
            if (item is SYSStationDto s && SelectedGroup != null)
            {
                // 若該站在 Targets，就顯示在右邊
                return SelectedGroup.Stations != null && SelectedGroup.Stations.Any(st => st.StationID.Equals(s.StationID));
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalSYSStationGroups == null || SYSStationGroups == null ||
                _originalSYSStationGroups.Count != SYSStationGroups.Count ||
                _originalSYSStationGroups.Zip(SYSStationGroups, (orig, curr) =>
                    orig.Id != curr.Id ||
                    orig.GroupName != curr.GroupName ||
                    (orig.Stations == null && curr.Stations != null) ||
                    (orig.Stations != null && curr.Stations == null) ||
                    (orig.Stations != null && curr.Stations != null &&
                        (orig.Stations.Count != curr.Stations.Count ||
                         orig.Stations.Zip(curr.Stations, (o, c) => !o.StationID.Equals(c.StationID)).Any(x => x))
                    )
                ).Any(x => x))
                return true;
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalSYSStationGroups = SYSStationGroups
                .Select(g => new SYSStationGroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    Stations = g.Stations == null ? null : g.Stations.Select(s => new SYSStationDto
                    {
                        StationID = s.StationID,
                        StationName = s.StationName
                    }).ToList()
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            // 更新 ViewModel 屬性
            SYSStationGroups = new ObservableCollection<SYSStationGroupDto>(DesignTimeDatas.SYSStationGroups);
            AllsysStations = new ObservableCollection<SYSStationDto>(DesignTimeDatas.SYSStations);

            // 主動指定選擇第一筆群組
            SelectedGroup = SYSStationGroups.FirstOrDefault();

            // 設定 CollectionView 筆刷
            var cvsUnselected = new CollectionViewSource { Source = AllsysStations };
            UnselectedStationsView = cvsUnselected.View;
            UnselectedStationsView.Filter = Unselected_Filter;

            var cvsSelected = new CollectionViewSource { Source = AllsysStations };
            SelectedStationsView = cvsSelected.View;
            SelectedStationsView.Filter = Selected_Filter;

            // 通知畫面更新

            //快照初始化
            UpdateSnapshot();
        }
        #endregion
    }
}
