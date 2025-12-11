using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.DMD.Datas;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.DMD.ViewModels
{
    public class DMDScheduleTemplateSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public DMDScheduleTemplateSettingsViewModel()
        {
        }
        public DMDScheduleTemplateSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        private List<DMDScheduleTemplateDto> _originalScheduleTemplates;

        public bool IsItemOperation => SelectedTemplate != null;

        private DMDScheduleTemplateDto _selectedTemplate;
        public DMDScheduleTemplateDto SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                SetProperty(ref _selectedTemplate, value);
                RaisePropertyChanged(nameof(IsItemOperation));
                RefreshStationViews();
            }
        }
        // 排程樣板 - 設定
        private ObservableCollection<DMDScheduleTemplateDto> _dmdScheduleTemplates = new();
        public ObservableCollection<DMDScheduleTemplateDto> DMDScheduleTemplates
        {
            get => _dmdScheduleTemplates;
            set => SetProperty(ref _dmdScheduleTemplates, value);
        }
        // 目標設定 - 車站列表
        private ObservableCollection<SYSStationGroupDto> _sysStationGroups = new();
        public ObservableCollection<SYSStationGroupDto> SYSStationGroups
        {
            get => _sysStationGroups;
            set => SetProperty(ref _sysStationGroups, value);
        }

        private SYSStationGroupDto _selectedStationGroup;
        public SYSStationGroupDto SelectedStationGroup
        {
            get => _selectedStationGroup;
            set
            {
                SetProperty(ref _selectedStationGroup, value);
                RefreshStationViews();
            }
        }

        private ObservableCollection<SYSStationDto> _allsysStations = new();
        public ObservableCollection<SYSStationDto> AllsysStations 
        {
            get => _allsysStations;
            set => SetProperty(ref _allsysStations, value);
        }

        public ICollectionView UnselectedStationsView { get; private set; }
        public ICollectionView SelectedStationsView { get; private set; }
        // 內容設定 - 預錄訊息
        private ObservableCollection<DMDMessageGroupDto> _dmdMessageGroups = new();
        public ObservableCollection<DMDMessageGroupDto> DMDMessageGroups
        {
            get => _dmdMessageGroups;
            set => SetProperty(ref _dmdMessageGroups, value);
        }

        private DMDMessageGroupDto _selectedMessageGroup;
        public DMDMessageGroupDto SelectedMessageGroup
        {
            get => _selectedMessageGroup;
            set => SetProperty(ref _selectedMessageGroup, value);
        }

        // 控件使用
        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    RaisePropertyChanged(nameof(SelectedTabIndex));
                }
            }
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {
            Guid guid = Guid.NewGuid();

            //畫面
            var newItem = new DMDScheduleTemplateDto
            {
                Id = guid,
                ScheduleName = "新排程",
                StartDayType = eDayType.當日,
                StartTime = new DateTime().Add(new TimeSpan(10, 30, 0)),
                EndDayType = eDayType.當日,
                EndTime = new DateTime().Add(new TimeSpan(10, 30, 0)),
                Content = null,
                Targets = new List<SYSStationDto>()
            };

            //資料庫

            DMDScheduleTemplates.Add(newItem);
            UpdateSnapshot();
            SelectedTemplate = newItem;
            SelectedTabIndex = 0;
        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {

            var itemToDelete = SelectedTemplate;

            if (itemToDelete == null)
                return;

            _dialogService.IsDelete($"確定要刪除\"{itemToDelete.ScheduleName}\"這筆樣板嗎？", result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //資料庫

                    SelectedTemplate = null;

                    DMDScheduleTemplates.Remove(itemToDelete);

                    UpdateSnapshot();
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedTemplate;

            //資料庫

            UpdateSnapshot();

            IsEditMode = false;
            SelectedTabIndex = 0;
        }

        private DelegateCommand _editCommand;
        public DelegateCommand EditCommand => _editCommand ??= new DelegateCommand(ExcuteEditCommand);
        private void ExcuteEditCommand()
        {
            IsEditMode = true;
            SelectedTabIndex = 0;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {
            if (!HasChanges())
            {
                IsEditMode = false;
                SelectedTabIndex = 0;
                return;
            }

            var restoredList = _originalScheduleTemplates
                .Select(o => new DMDScheduleTemplateDto
                {
                    Id = o.Id,
                    ScheduleName = o.ScheduleName,
                    StartDayType = o.StartDayType,
                    StartTime = o.StartTime,
                    EndDayType = o.EndDayType,
                    EndTime = o.EndTime,

                    Content = o.Content == null
                        ? null
                        : new DMDPreRecordMessageDto
                        {
                            Id = o.Content.Id,
                            MessageName = o.Content.MessageName,
                            MessageContent = o.Content.MessageContent,
                            // 這裡要注意：如果 o.Content.BelongGroup 也是個 DTO，建議也 new 一份（深拷貝群組）
                            BelongGroup = o.Content.BelongGroup == null
                                ? null
                                : new DMDMessageGroupDto
                                {
                                    Id = o.Content.BelongGroup.Id,
                                    GroupName = o.Content.BelongGroup.GroupName,
                                    Messages = o.Content.BelongGroup.Messages
                                },
                            IsUseDU = o.Content.IsUseDU
                        },

                    Targets = o.Targets == null
                        ? new List<SYSStationDto>()
                        : o.Targets
                            .Select(t => new SYSStationDto
                            {
                                StationID = t.StationID,
                                StationName = t.StationName,
                                // 同樣，若 BelongGroup 是複合物件，就 new 一份
                                BelongGroup = t.BelongGroup == null
                                    ? null
                                    : new SYSStationGroupDto
                                    {
                                        Id = t.BelongGroup.Id,
                                        GroupName = t.BelongGroup.GroupName,
                                        Stations = t.BelongGroup.Stations
                                    }
                            })
                            .ToList()
                })
                .ToList();

            DMDScheduleTemplates = new ObservableCollection<DMDScheduleTemplateDto>(restoredList);
            RaisePropertyChanged(nameof(DMDScheduleTemplates));

            SelectedTemplate = DMDScheduleTemplates.FirstOrDefault();
            RaisePropertyChanged(nameof(SelectedTemplate));

            IsEditMode = false;
            SelectedTabIndex = 0;
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
        // 車站選擇
        private DelegateCommand<SYSStationDto> _selectStationCommand;
        public DelegateCommand<SYSStationDto> SelectStationCommand =>
            _selectStationCommand ??= new DelegateCommand<SYSStationDto>(ExecuteSelectStationCommand);
        private void ExecuteSelectStationCommand(SYSStationDto station)
        {
            if (station == null || SelectedTemplate == null) return;

            // 用 StationID 找到要移除的物件
            var exist = SelectedTemplate.Targets.FirstOrDefault(t => t.StationID == station.StationID);
            if (exist != null)
                SelectedTemplate.Targets.Remove(exist);
            else
                SelectedTemplate.Targets.Add(station);

            RefreshStationViews();
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
            if (!(item is SYSStationDto s) || SelectedTemplate == null || SelectedStationGroup == null)
                return false;

            // 如果「全部車站」被選中，就不需要進一步檢查 BelongGroup：
            if (SelectedStationGroup.GroupName == "全部車站")
            {
                // 只需排除已在 Targets 中的車站
                return !SelectedTemplate.Targets.Any(t => t.StationID == s.StationID);
            }

            // 如果不是「全部群組」，再檢查 s.BelongGroup 是否為 null
            if (s.BelongGroup == null)
                return false;

            // 比對 s 屬於的群組名稱是否跟 SelectedStationGroup 相同
            if (s.BelongGroup.GroupName != SelectedStationGroup.GroupName)
                return false;

            // 若通過群組比對，再排除已在 Targets 中的車站
            return !SelectedTemplate.Targets.Any(t => t.StationID == s.StationID);
        }
        private bool Selected_Filter(object item)
        {
            if (item is SYSStationDto s && SelectedTemplate != null)
            {
                // 若該站在 Targets，就顯示在右邊
                return SelectedTemplate.Targets.Any(t => t.StationID == s.StationID);
            }
            return false;
        }
        private bool HasChanges()
        {
            if (_originalScheduleTemplates.Count != DMDScheduleTemplates.Count)
                return true;

            foreach (var current in DMDScheduleTemplates)
            {
                var original = _originalScheduleTemplates.FirstOrDefault(o => o.Id == current.Id);
                if (original == null)
                    return true;

                if (original.ScheduleName != current.ScheduleName ||
                    original.StartDayType != current.StartDayType ||
                    original.StartTime != current.StartTime ||
                    original.EndDayType != current.EndDayType ||
                    original.EndTime != current.EndTime)
                {
                    return true;
                }

                // 比對 Content（判斷有無 null）
                if ((original.Content == null && current.Content != null) ||
                    (original.Content != null && current.Content == null))
                {
                    return true;
                }

                // 比對 Targets：筆數不同 → 有變動
                var origTargets = original.Targets ?? Enumerable.Empty<SYSStationDto>();
                var currTargets = current.Targets ?? Enumerable.Empty<SYSStationDto>();
                if (origTargets.Count() != currTargets.Count())
                    return true;

            }

            // 如果以上都沒找到任何差異 → 視為無變動
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalScheduleTemplates = DMDScheduleTemplates
        .Select(m => new DMDScheduleTemplateDto
        {
            // 複製最外層基本欄位（值型別或字串都可以直接指派）
            Id = m.Id,
            ScheduleName = m.ScheduleName,
            StartDayType = m.StartDayType,
            StartTime = m.StartTime,
            EndDayType = m.EndDayType,
            EndTime = m.EndTime,

            // 對 Content 做深拷貝：new 一個新的 DMDPreRecordMessageDto
            Content = m.Content == null
                ? null
                : new DMDPreRecordMessageDto
                {
                    Id = m.Content.Id,
                    MessageName = m.Content.MessageName,
                    MessageContent = m.Content.MessageContent,
                    BelongGroup = m.Content.BelongGroup,
                    IsUseDU = m.Content.IsUseDU
                },

            // 對 Targets 做深拷貝：new 一個 List<SYSStationDto>，並把每個子項都複製一份
            Targets = m.Targets == null
                ? new List<SYSStationDto>()
                : m.Targets
                    .Select(s => new SYSStationDto
                    {
                        StationID = s.StationID,
                        StationName = s.StationName,
                        BelongGroup = s.BelongGroup,
                    })
                    .ToList()
        })
        .ToList();

        }
        private async Task InitDataAsync()
        {
            //讀取DMDScheduleTemplateDto
            //讀取DMDPreRecordMessages
            //讀取SYSStations.
            //讀取後分類與組裝


            // 更新 ViewModel 屬性
            DMDScheduleTemplates = new ObservableCollection<DMDScheduleTemplateDto>(DesignTimeDatas.DMDScheduleTemplates);
            SYSStationGroups = new ObservableCollection<SYSStationGroupDto>(DesignTimeDatas.SYSStationGroups);
            AllsysStations = new ObservableCollection<SYSStationDto>(DesignTimeDatas.SYSStations);
            DMDMessageGroups = new ObservableCollection<DMDMessageGroupDto>(DesignTimeDatas.DMDMessageGroups);

            // 主動指定選擇第一筆群組
            SelectedTemplate = DMDScheduleTemplates.FirstOrDefault();
            SelectedStationGroup = SYSStationGroups.FirstOrDefault();
            SelectedMessageGroup = DMDMessageGroups.FirstOrDefault();

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
