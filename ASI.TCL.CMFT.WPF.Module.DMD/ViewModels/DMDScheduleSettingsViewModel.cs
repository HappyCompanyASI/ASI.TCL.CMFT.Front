using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.DMD.Datas;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.DMD.ViewModels
{
    public class DMDScheduleSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
   
        #region Constructors
        public DMDScheduleSettingsViewModel()
        {
        }
        public DMDScheduleSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<DMDScheduleTemplateDto> _originalSchedules;

        //樣板清單
        private ObservableCollection<DMDScheduleTemplateDto> _dmdScheduleTemplates = new();
        public ObservableCollection<DMDScheduleTemplateDto> DMDScheduleTemplates
        {
            get => _dmdScheduleTemplates;
            set => SetProperty(ref _dmdScheduleTemplates, value);
        }

        private DMDScheduleTemplateDto _selectedTemplate;
        public DMDScheduleTemplateDto SelectedTemplate
        {
            get => _selectedTemplate;
            set => SetProperty(ref _selectedTemplate, value);
        }

        //今日排程
        private ObservableCollection<DMDScheduleTemplateDto> _dmdSchedules = new();
        public ObservableCollection<DMDScheduleTemplateDto> DMDSchedules
        {
            get => _dmdSchedules;
            set => SetProperty(ref _dmdSchedules, value);
        }

        private DMDScheduleTemplateDto _selectedSchedule;
        public DMDScheduleTemplateDto SelectedSchedule
        {
            get => _selectedSchedule;
            set => SetProperty(ref _selectedSchedule, value);
        }

        //前七日排程預覽
        private ObservableCollection<DMDDayScheduleDto> _dmdWeekSchedules = new ();
        public ObservableCollection<DMDDayScheduleDto> DMDDaySchedules
        {
            get => _dmdWeekSchedules;
            set => SetProperty(ref _dmdWeekSchedules, value);
        }

        private DMDDayScheduleDto _selectedDaySchedule;
        public DMDDayScheduleDto SelectedDaySchedule
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
            var itemToUpdate = DMDSchedules;

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

            DMDSchedules = new ObservableCollection<DMDScheduleTemplateDto>(restoredList);
            RaisePropertyChanged(nameof(DMDSchedules));

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

            DMDSchedules.Add(itemToAdd);
        }
        private DelegateCommand _deleteScheduleTemplateCommand;
        public DelegateCommand DeleteScheduleTemplateCommand => _deleteScheduleTemplateCommand ??= new DelegateCommand(ExcuteDeleteScheduleTemplateCommand);
        private void ExcuteDeleteScheduleTemplateCommand()
        {
            var itemToDelete = SelectedSchedule;

            if (itemToDelete == null)
                return;

            SelectedSchedule = null;

            DMDSchedules.Remove(itemToDelete);
        }
        private DelegateCommand _applyDaySchedulesCommand;
        public DelegateCommand ApplyDaySchedulesCommand => _applyDaySchedulesCommand ??= new DelegateCommand(ExcuteApplyDaySchedulesCommand);
        private void ExcuteApplyDaySchedulesCommand()
        {
            var itemToApply = SelectedDaySchedule;

            if (itemToApply == null)
                return;

            DMDSchedules = new ObservableCollection<DMDScheduleTemplateDto>(SelectedDaySchedule.Templates);
        }
        #endregion

        #region Private Methods
        private bool HasChanges()
        {
            if (_originalSchedules.Count != DMDSchedules.Count)
                return true;

            foreach (var current in DMDSchedules)
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
            _originalSchedules = DMDSchedules
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
            //讀取DMDScheduleTemplates
            //讀取DMDSchedules
            //讀取DMDDaySchedules
            //讀取後分類與組裝

            // 更新 ViewModel 屬性
            DMDScheduleTemplates = new ObservableCollection<DMDScheduleTemplateDto>(DesignTimeDatas.DMDScheduleTemplates);
            DMDSchedules = new ObservableCollection<DMDScheduleTemplateDto>(DesignTimeDatas.DMDScheduleTemplates);
            DMDDaySchedules = new ObservableCollection<DMDDayScheduleDto>(DesignTimeDatas.DMDDaySchedules);

            // 主動指定選擇第一筆群組

            // 通知畫面更新

            //快照初始化
            UpdateSnapshot();
        }
        #endregion
    }
}
