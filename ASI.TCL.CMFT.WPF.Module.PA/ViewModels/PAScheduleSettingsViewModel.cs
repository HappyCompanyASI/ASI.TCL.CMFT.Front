using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.PA.Datas;
using ASI.TCL.CMFT.WPF.Module.PA.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PAScheduleSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
   
        #region Constructors
        public PAScheduleSettingsViewModel()
        {
        }
        public PAScheduleSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<PAScheduleTemplateDto> _originalSchedules;

        //樣板清單
        private ObservableCollection<PAScheduleTemplateDto> _paScheduleTemplates = new();
        public ObservableCollection<PAScheduleTemplateDto> PAScheduleTemplates
        {
            get => _paScheduleTemplates;
            set => SetProperty(ref _paScheduleTemplates, value);
        }

        private PAScheduleTemplateDto _selectedTemplate;
        public PAScheduleTemplateDto SelectedTemplate
        {
            get => _selectedTemplate;
            set => SetProperty(ref _selectedTemplate, value);
        }

        //今日排程
        private ObservableCollection<PAScheduleTemplateDto> _paSchedules = new();
        public ObservableCollection<PAScheduleTemplateDto> PASchedules
        {
            get => _paSchedules;
            set => SetProperty(ref _paSchedules, value);
        }

        private PAScheduleTemplateDto _selectedSchedule;
        public PAScheduleTemplateDto SelectedSchedule
        {
            get => _selectedSchedule;
            set => SetProperty(ref _selectedSchedule, value);
        }

        //前七日排程預覽
        private ObservableCollection<PADayScheduleDto> _paWeekSchedules = new();
        public ObservableCollection<PADayScheduleDto> PADaySchedules
        {
            get => _paWeekSchedules;
            set => SetProperty(ref _paWeekSchedules, value);
        }

        private PADayScheduleDto _selectedDaySchedule;
        public PADayScheduleDto SelectedDaySchedule
        {
            get => _selectedDaySchedule;
            set => SetProperty(ref _selectedDaySchedule, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = PASchedules;

            //資料庫

            UpdateSnapshot();

            SelectedSchedule = null;
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

            var restoredList = _originalSchedules
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

            PASchedules = new ObservableCollection<PAScheduleTemplateDto>(restoredList);
            RaisePropertyChanged(nameof(PASchedules));

            SelectedSchedule = null;

            IsEditMode = false;
        }

        private DelegateCommand _addScheduleTemplateCommand;
        public DelegateCommand AddScheduleTemplateCommand => _addScheduleTemplateCommand ??= new DelegateCommand(ExcuteAddScheduleTemplateCommand);
        private void ExcuteAddScheduleTemplateCommand()
        {
            var itemToAdd = SelectedTemplate;

            if (itemToAdd == null)
                return;

            PASchedules.Add(itemToAdd);
        }
        private DelegateCommand _deleteScheduleTemplateCommand;
        public DelegateCommand DeleteScheduleTemplateCommand => _deleteScheduleTemplateCommand ??= new DelegateCommand(ExcuteDeleteScheduleTemplateCommand);
        private void ExcuteDeleteScheduleTemplateCommand()
        {
            var itemToDelete = SelectedSchedule;

            if (itemToDelete == null)
                return;

            SelectedSchedule = null;

            PASchedules.Remove(itemToDelete);
        }
        private DelegateCommand _applyDaySchedulesCommand;
        public DelegateCommand ApplyDaySchedulesCommand => _applyDaySchedulesCommand ??= new DelegateCommand(ExcuteApplyDaySchedulesCommand);
        private void ExcuteApplyDaySchedulesCommand()
        {
            var itemToApply = SelectedDaySchedule;

            if (itemToApply == null)
                return;

            PASchedules = new ObservableCollection<PAScheduleTemplateDto>(SelectedDaySchedule.Templates);
        }
        #endregion

        #region Private Methods
        private bool HasChanges()
        {
            if (_originalSchedules.Count != PASchedules.Count)
                return true;

            foreach (var current in PASchedules)
            {
                var original = _originalSchedules.FirstOrDefault(o => o.Id == current.Id);
                if (original == null)
                    return true;
            }

            // 如果以上都沒找到任何差異 → 視為無變動
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalSchedules = PASchedules
               .Select(m => new PAScheduleTemplateDto
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
                    : new PAPreRecordVoiceDto
                    {
                        Id = m.Content.Id,
                        VoiceName = m.Content.VoiceName,
                        VoiceContent = m.Content.VoiceContent,
                        IsIncludeCHN = m.Content.IsIncludeCHN,
                        IsIncludeTWN = m.Content.IsIncludeTWN,
                        IsIncludeHAKKA = m.Content.IsIncludeHAKKA,
                        IsIncludeENG = m.Content.IsIncludeENG,
                        BelongGroup = m.Content.BelongGroup == null
                            ? null
                            : new PAVoiceGroupDto
                            {
                                Id = m.Content.BelongGroup.Id,
                                GroupName = m.Content.BelongGroup.GroupName,
                                Voices = m.Content.BelongGroup.Voices
                            }
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
            //讀取PAScheduleTemplates
            //讀取PASchedules
            //讀取PADaySchedules
            //讀取後分類與組裝

            // 更新 ViewModel 屬性
            PAScheduleTemplates = new ObservableCollection<PAScheduleTemplateDto>(DesignTimeDatas.PAScheduleTemplates);
            PASchedules = new ObservableCollection<PAScheduleTemplateDto>(DesignTimeDatas.PAScheduleTemplates);
            PADaySchedules = new ObservableCollection<PADayScheduleDto>(DesignTimeDatas.PADaySchedules);

            // 主動指定選擇第一筆群組

            // 通知畫面更新

            //快照初始化
            UpdateSnapshot();
        }
        #endregion

    }
}
