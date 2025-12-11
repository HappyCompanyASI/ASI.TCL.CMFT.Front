using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.Alarm.Datas;
using ASI.TCL.CMFT.WPF.Module.Alarm.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.Alarm.ViewModels
{
    public class AlarmLevelDefinitionSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public AlarmLevelDefinitionSettingsViewModel()
        {
        }
        public AlarmLevelDefinitionSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<AlarmLevelDefineDto> _originalAlarmLevelDefines;

        public bool IsItemOperation => SelectedItem != null;

        private AlarmLevelDefineDto _selectedItem;
        public AlarmLevelDefineDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        private ObservableCollection<eAlarmSound> _soundsOptions;
        public ObservableCollection<eAlarmSound> SoundsOptions => _soundsOptions ??= new ObservableCollection<eAlarmSound>(
                Enum.GetValues(typeof(eAlarmSound))
                    .Cast<eAlarmSound>());

        private ObservableCollection<String> _colorsOptions;
        public ObservableCollection<String> ColorsOptions =>
            _colorsOptions ??= new ObservableCollection<String>(DesignTimeDatas.AlarmLevelColors);

        // 告警等級資料
        private ObservableCollection<AlarmLevelDefineDto> _alarmLevelDefines = new();
        public ObservableCollection<AlarmLevelDefineDto> AlarmLevelDefines
        {
            get => _alarmLevelDefines;
            set => SetProperty(ref _alarmLevelDefines, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedItem;

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

            // 還原資料
            AlarmLevelDefines = new ObservableCollection<AlarmLevelDefineDto>(
                _originalAlarmLevelDefines.Select(x => new AlarmLevelDefineDto
                {
                    AlarmLevel = x.AlarmLevel,
                    IsNeedConfirm = x.IsNeedConfirm,
                    Color = x.Color,
                    AlarmSound = x.AlarmSound,
                    IsFlashing = x.IsFlashing,
                    IsToSCADA = x.IsToSCADA
                })
            );

            RaisePropertyChanged(nameof(AlarmLevelDefines));

            IsEditMode = false;
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
        private bool HasChanges()
        {
            if (_originalAlarmLevelDefines == null || AlarmLevelDefines == null)
                return false;

            if (_originalAlarmLevelDefines.Count != AlarmLevelDefines.Count)
                return true;

            for (int i = 0; i < AlarmLevelDefines.Count; i++)
            {
                var curr = AlarmLevelDefines[i];
                var orig = _originalAlarmLevelDefines[i];
                if (curr.AlarmLevel != orig.AlarmLevel
                    || curr.IsNeedConfirm != orig.IsNeedConfirm
                    || curr.Color != orig.Color
                    || curr.AlarmSound != orig.AlarmSound
                    || curr.IsFlashing != orig.IsFlashing
                    || curr.IsToSCADA != orig.IsToSCADA)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalAlarmLevelDefines = AlarmLevelDefines
                .Select(x => new AlarmLevelDefineDto
                {
                    AlarmLevel = x.AlarmLevel,
                    IsNeedConfirm = x.IsNeedConfirm,
                    Color = x.Color,
                    AlarmSound = x.AlarmSound,
                    IsFlashing = x.IsFlashing,
                    IsToSCADA = x.IsToSCADA
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            AlarmLevelDefines = new ObservableCollection<AlarmLevelDefineDto>(DesignTimeDatas.AlarmLevelDefines);
            SelectedItem = DesignTimeDatas.AlarmLevelDefines.FirstOrDefault();

            UpdateSnapshot();
        }
        #endregion
    }
}
