using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    public class PABroadcastTimeAdjustSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;
   

        #region Constructors
        public PABroadcastTimeAdjustSettingsViewModel()
        {
        }
        public PABroadcastTimeAdjustSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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
            set
            {
                SetProperty(ref _isEditMode, value);
                RaisePropertyChanged(nameof(CanItemAddDelete));
                RaisePropertyChanged(nameof(IsGroupOperation));
            }
        }

        private List<PAStationBroadcastScheduleDto> _originalSchedules;

        public bool IsGroupOperation => SelectedDailyBroadcast != null && !IsEditMode;
        public bool IsItemOperation => SelectedTimeSlot != null;
        public bool CanItemAddDelete => SelectedTimeSlot != null && !IsEditMode;

        private PAStationBroadcastScheduleDto _selectedStationBroadcast;
        public PAStationBroadcastScheduleDto SelectedStationBroadcast
        {
            get => _selectedStationBroadcast;
            set => SetProperty(ref _selectedStationBroadcast, value);
        }

        private PADailyBroadcastScheduleDto _selectedDailyBroadcast;
        public PADailyBroadcastScheduleDto SelectedDailyBroadcast
        {
            get => _selectedDailyBroadcast;
            set
            {
                SetProperty(ref _selectedDailyBroadcast, value);
                RaisePropertyChanged(nameof(IsGroupOperation));
            }
        }

        private PATimeSlotDto _selectedTimeSlot;
        public PATimeSlotDto SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                SetProperty(ref _selectedTimeSlot, value);
                RaisePropertyChanged(nameof(IsItemOperation));
                RaisePropertyChanged(nameof(CanItemAddDelete));
            }
        }

        // 車站廣播排程資料
        private ObservableCollection<PAStationBroadcastScheduleDto> _paStationBroadcastSchedules = new();
        public ObservableCollection<PAStationBroadcastScheduleDto> PAStationBroadcastSchedules
        {
            get => _paStationBroadcastSchedules;
            set => SetProperty(ref _paStationBroadcastSchedules, value);
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

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {

            if (SelectedDailyBroadcast == null)
                return;

            var newSlot = new PATimeSlotDto
            {
                Id = Guid.NewGuid(),
                DailyScheduleId = SelectedDailyBroadcast.Id,
                StartDayType = eDayType.當日,
                StartTime = DateTime.Today,
                EndDayType = eDayType.當日,
                EndTime = DateTime.Today.AddHours(1),
                DayPartType = eDaypartType.離峰時段
            };

            //資料庫

            SelectedDailyBroadcast.DaypartTimes.Add(newSlot);
            UpdateSnapshot();

            SelectedTimeSlot = newSlot;

        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {
            if (SelectedDailyBroadcast == null || SelectedTimeSlot == null)
                return;

            //資料庫

            SelectedDailyBroadcast.DaypartTimes.Remove(SelectedTimeSlot);
            UpdateSnapshot();

            SelectedTimeSlot = null;
        }
        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedDailyBroadcast;

            //資料庫

            UpdateSnapshot();

            SelectedTimeSlot = null;
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

            var oldStationId = SelectedStationBroadcast?.Id;
            var oldDay = SelectedDailyBroadcast?.Day;

            // 1. 用 LINQ 深拷三層 DTO 回一個新的 List
            var restoredList = _originalSchedules
                .Select(o => new PAStationBroadcastScheduleDto
                {
                    // 最外層
                    Id = o.Id,
                    StationId = o.StationId,
                    Station = o.Station == null
                        ? null
                        : new SYSStationDto
                        {
                            StationID = o.Station.StationID,
                            StationName = o.Station.StationName,
                            BelongGroup = o.Station.BelongGroup
                        },

                    // 中層 DailySchedules
                    DailySchedules = o.DailySchedules == null
                        ? new List<PADailyBroadcastScheduleDto>()
                        : o.DailySchedules
                            .Select(d => new PADailyBroadcastScheduleDto
                            {
                                Id = d.Id,
                                StationScheduleId = d.StationScheduleId,
                                Day = d.Day,
                                // DayDisplay 只讀，不需拷貝
                                DaypartTimes = d.DaypartTimes == null
                                    ? new List<PATimeSlotDto>()
                                    : d.DaypartTimes
                                        .Select(t => new PATimeSlotDto
                                        {
                                            Id = t.Id,
                                            DailyScheduleId = t.DailyScheduleId,
                                            StartDayType = t.StartDayType,
                                            StartTime = t.StartTime,
                                            EndDayType = t.EndDayType,
                                            EndTime = t.EndTime,
                                            DayPartType = t.DayPartType
                                        })
                                        .ToList()
                            })
                            .ToList()
                })
                .ToList();

            // 2. 把新的 List 包回三層 ObservableCollection
            foreach (var stationDto in restoredList)
            {
                // 外層 DailySchedules
                stationDto.DailySchedules = new ObservableCollection<PADailyBroadcastScheduleDto>(
                    stationDto.DailySchedules
                );

                // 內層 DaypartTimes
                foreach (var dailyDto in stationDto.DailySchedules)
                {
                    dailyDto.DaypartTimes = new ObservableCollection<PATimeSlotDto>(
                        dailyDto.DaypartTimes
                    );
                }
            }

            // 3. 套回 ViewModel 屬性
            PAStationBroadcastSchedules = new ObservableCollection<PAStationBroadcastScheduleDto>(
                restoredList
            );
            RaisePropertyChanged(nameof(PAStationBroadcastSchedules));

            // 4. 重設選取與模式
            SelectedStationBroadcast = PAStationBroadcastSchedules
                .FirstOrDefault(s => s.Id == oldStationId);
            SelectedDailyBroadcast = SelectedStationBroadcast?
                .DailySchedules
                .FirstOrDefault(d => d.Day == oldDay);

            IsEditMode = false;
        }
        #endregion

        #region Private Methods
        private bool HasChanges()
        {
            // 1. 只比最裡層所有時段的總數
            int currentSlotCount = PAStationBroadcastSchedules
                .SelectMany(st => st.DailySchedules)
                .SelectMany(d => d.DaypartTimes)
                .Count();

            int originalSlotCount = _originalSchedules
                .SelectMany(st => st.DailySchedules)
                .SelectMany(d => d.DaypartTimes)
                .Count();

            if (currentSlotCount != originalSlotCount)
                return true;   // 時段數量有變化

            // 2. 檢查最裡層每個時段的內容是否有改動
            foreach (var current in PAStationBroadcastSchedules)
            {
                // 找到相同 StationSchedule
                var origStation = _originalSchedules.FirstOrDefault(o => o.Id == current.Id);
                if (origStation == null)
                    return true;

                foreach (var currDaily in current.DailySchedules)
                {
                    // 找到相同的每日排程
                    var origDaily = origStation.DailySchedules
                        .FirstOrDefault(d => d.Day == currDaily.Day);
                    if (origDaily == null)
                        return true;

                    // 比對每個時段
                    foreach (var currSlot in currDaily.DaypartTimes)
                    {
                        var origSlot = origDaily.DaypartTimes
                            .FirstOrDefault(s => s.Id == currSlot.Id);
                        if (origSlot == null)
                            return true;  // 新增或刪除時段

                        // 比對屬性
                        if (origSlot.StartDayType != currSlot.StartDayType
                         || origSlot.StartTime != currSlot.StartTime
                         || origSlot.EndDayType != currSlot.EndDayType
                         || origSlot.EndTime != currSlot.EndTime
                         || origSlot.DayPartType != currSlot.DayPartType)
                        {
                            return true;  // 時間或類型有改動
                        }
                    }
                }
            }
            // 沒發現任何變動
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalSchedules = PAStationBroadcastSchedules
            // PAStationBroadcastSchedules 是 ObservableCollection<PAStationBroadcastScheduleDto>
            .Select(st => new PAStationBroadcastScheduleDto
            {
                // 最外層欄位
                Id = st.Id,
                StationId = st.StationId,
                Station = st.Station == null
                    ? null
                    : new SYSStationDto
                    {
                        StationID = st.Station.StationID,
                        StationName = st.Station.StationName,
                        BelongGroup = st.Station.BelongGroup,
                        // …若 SYSStationDto 還有其他欄位也一起拷貝
                    },

                // 中層：DailySchedules 深拷貝
                DailySchedules = st.DailySchedules == null
                    ? new List<PADailyBroadcastScheduleDto>()
                    : st.DailySchedules
                        .Select(d => new PADailyBroadcastScheduleDto
                        {
                            Id = d.Id,
                            StationScheduleId = d.StationScheduleId,
                            Day = d.Day,
                            // DayDisplay 是唯讀計算屬性，不需要手動拷貝

                            // 最裡層：DaypartTimes 深拷貝
                            DaypartTimes = d.DaypartTimes == null
                                ? new List<PATimeSlotDto>()
                                : d.DaypartTimes
                                    .Select(t => new PATimeSlotDto
                                    {
                                        Id = t.Id,
                                        DailyScheduleId = t.DailyScheduleId,
                                        StartDayType = t.StartDayType,
                                        StartTime = t.StartTime,
                                        EndDayType = t.EndDayType,
                                        EndTime = t.EndTime,
                                        DayPartType = t.DayPartType
                                    })
                                    .ToList()
                        })
                        .ToList()
            })
            .ToList();
        }
        private async Task InitDataAsync()
        {

            // 1. 先把假資料拉出來變成 List，好做後續修改
            var stationList = DesignTimeDatas
                .PAStationBroadcastSchedules
                .ToList();
            // 2. 把每一筆 DTO 的 DailySchedules、DaypartTimes 都包成 ObservableCollection
            foreach (var stationDto in stationList)
            {
                // 包 DailySchedules
                stationDto.DailySchedules = new ObservableCollection<PADailyBroadcastScheduleDto>(
                    stationDto.DailySchedules
                );

                // 包 DaypartTimes
                foreach (var dailyDto in stationDto.DailySchedules)
                {
                    dailyDto.DaypartTimes = new ObservableCollection<PATimeSlotDto>(
                        dailyDto.DaypartTimes
                    );
                }
            }
            // 3. 最後再整個丟進 ViewModel 屬性
            PAStationBroadcastSchedules = new ObservableCollection<PAStationBroadcastScheduleDto>(
                stationList
            );


            // 更新 ViewModel 屬性

            // 主動指定選擇第一筆群組

            // 設定 CollectionView 筆刷

            // 通知畫面更新

            //快照初始化
            UpdateSnapshot();
        }
        #endregion
    }
}
