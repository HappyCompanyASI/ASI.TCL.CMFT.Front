using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.PA.Datas;
using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;
using ASI.TCL.CMFT.WPF.Module.PA.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PAScheduleTemplateSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;


        #region Constructors
        public PAScheduleTemplateSettingsViewModel()
        {
        }
        public PAScheduleTemplateSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<PAScheduleTemplateDto> _originalScheduleTemplates;

        public bool IsItemOperation => SelectedTemplate != null;

        private PAScheduleTemplateDto _selectedTemplate;
        public PAScheduleTemplateDto SelectedTemplate
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
        private ObservableCollection<PAScheduleTemplateDto> _paScheduleTemplates = new();
        public ObservableCollection<PAScheduleTemplateDto> PAScheduleTemplates
        {
            get => _paScheduleTemplates;
            set => SetProperty(ref _paScheduleTemplates, value);
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
        private ObservableCollection<PAVoiceGroupDto> _paVoiceGroups = new();
        public ObservableCollection<PAVoiceGroupDto> PAVoiceGroups
        {
            get => _paVoiceGroups;
            set => SetProperty(ref _paVoiceGroups, value);
        }

        private PAVoiceGroupDto _selectedVoiceGroup;
        public PAVoiceGroupDto SelectedVoiceGroup
        {
            get => _selectedVoiceGroup;
            set => SetProperty(ref _selectedVoiceGroup, value);
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

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {
            Guid guid = Guid.NewGuid();

            //畫面
            var newItem = new PAScheduleTemplateDto
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

            PAScheduleTemplates.Add(newItem);
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

                    PAScheduleTemplates.Remove(itemToDelete);

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
                .Select(o => new PAScheduleTemplateDto
                {
                    Id = o.Id,
                    ScheduleName = o.ScheduleName,
                    StartDayType = o.StartDayType,
                    StartTime = o.StartTime,
                    EndDayType = o.EndDayType,
                    EndTime = o.EndTime,

                    Content = o.Content == null
                        ? null
                        : new PAPreRecordVoiceDto
                        {
                            Id = o.Content.Id,
                            VoiceName = o.Content.VoiceName,
                            VoiceContent = o.Content.VoiceContent,
                            IsIncludeCHN = o.Content.IsIncludeCHN,
                            IsIncludeTWN = o.Content.IsIncludeTWN,
                            IsIncludeHAKKA = o.Content.IsIncludeHAKKA,
                            IsIncludeENG = o.Content.IsIncludeENG,
                            BelongGroup = o.Content.BelongGroup == null
                                ? null
                                : new PAVoiceGroupDto
                                {
                                    Id = o.Content.BelongGroup.Id,
                                    GroupName = o.Content.BelongGroup.GroupName,
                                    Voices = o.Content.BelongGroup.Voices
                                }
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

            PAScheduleTemplates = new ObservableCollection<PAScheduleTemplateDto>(restoredList);
            RaisePropertyChanged(nameof(PAScheduleTemplates));

            SelectedTemplate = PAScheduleTemplates.FirstOrDefault();
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
            if (_originalScheduleTemplates.Count != PAScheduleTemplates.Count)
                return true;

            foreach (var current in PAScheduleTemplates)
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
            _originalScheduleTemplates = PAScheduleTemplates
        .Select(m => new PAScheduleTemplateDto
        {
            // 複製最外層基本欄位（值型別或字串都可以直接指派）
            Id = m.Id,
            ScheduleName = m.ScheduleName,
            StartDayType = m.StartDayType,
            StartTime = m.StartTime,
            EndDayType = m.EndDayType,
            EndTime = m.EndTime,

            // 將 UpdateSnapshot 方法中的 Content 屬性深拷貝部分，依據 PAPreRecordVoiceDto 的屬性正確映射
            Content = m.Content == null
                ? null
                : new PAPreRecordVoiceDto
                {
                    Id = m.Content.Id,
                    VoiceName = m.Content.VoiceName,
                    VoiceContent = m.Content.VoiceContent,
                    IsIncludeCHN = m.Content.IsIncludeCHN,
                    IsIncludeTWN = m.Content.IsIncludeTWN,
                    IsIncludeHAKKA = m.Content.IsIncludeHAKKA,
                    IsIncludeENG = m.Content.IsIncludeENG,
                    BelongGroup = m.Content.BelongGroup
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
            //讀取PAScheduleTemplateDto
            //讀取PAPreRecordMessages
            //讀取SYSStations.
            //讀取後分類與組裝


            // 更新 ViewModel 屬性
            PAScheduleTemplates = new ObservableCollection<PAScheduleTemplateDto>(DesignTimeDatas.PAScheduleTemplates);
            SYSStationGroups = new ObservableCollection<SYSStationGroupDto>(DesignTimeDatas.SYSStationGroups);
            AllsysStations = new ObservableCollection<SYSStationDto>(DesignTimeDatas.SYSStations);
            PAVoiceGroups = new ObservableCollection<PAVoiceGroupDto>(DesignTimeDatas.PAVoiceGroups);

            // 主動指定選擇第一筆群組
            SelectedTemplate = PAScheduleTemplates.FirstOrDefault();
            SelectedStationGroup = SYSStationGroups.FirstOrDefault();
            SelectedVoiceGroup = PAVoiceGroups.FirstOrDefault();

            // 設定 CollectionView 筆刷
            var cvsUnselected = new CollectionViewSource { Source = AllsysStations };
            UnselectedStationsView = cvsUnselected.View;
            UnselectedStationsView.Filter = Unselected_Filter;

            var cvsSelected = new CollectionViewSource { Source = AllsysStations };
            SelectedStationsView = cvsSelected.View;
            SelectedStationsView.Filter = Selected_Filter;

            //快照初始化
            UpdateSnapshot();
        }
        #endregion
    }
}
