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
    public class AlarmTypeDefinitionSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public AlarmTypeDefinitionSettingsViewModel()
        {
        }
        public AlarmTypeDefinitionSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<EquipAlarmTypeDefineDto> _originalAlarmTypeDefines;

        public bool IsItemOperation => SelectedItem != null;

        private EquipAlarmTypeDefineDto _selectedItem;
        public EquipAlarmTypeDefineDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        // 告警等級資料
        private ObservableCollection<AlarmLevelDefineDto> _alarmLevelDefines = new();
        public ObservableCollection<AlarmLevelDefineDto> AlarmLevelDefines
        {
            get => _alarmLevelDefines;
            set => SetProperty(ref _alarmLevelDefines, value);
        }

        // 告警類型資料
        private ObservableCollection<EquipAlarmTypeDefineDto> _alarmTypeDefines = new();
        public ObservableCollection<EquipAlarmTypeDefineDto> AlarmTypeDefines
        {
            get => _alarmTypeDefines;
            set => SetProperty(ref _alarmTypeDefines, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private  void ExcuteConfirmCommand()
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
            AlarmTypeDefines = new ObservableCollection<EquipAlarmTypeDefineDto>(
                _originalAlarmTypeDefines.Select(x => new EquipAlarmTypeDefineDto
                {
                    EquipAlarmTypeID = x.EquipAlarmTypeID,
                    EquipType = x.EquipType,
                    Description = x.Description,
                    DescriptionENG = x.DescriptionENG,
                    PossibleReason = x.PossibleReason,
                    PossibleReasonENG = x.PossibleReasonENG,
                    AlarmLevelDefine = x.AlarmLevelDefine == null ? null : new AlarmLevelDefineDto
                    {
                        AlarmLevel = x.AlarmLevelDefine.AlarmLevel,
                        IsNeedConfirm = x.AlarmLevelDefine.IsNeedConfirm,
                        Color = x.AlarmLevelDefine.Color,
                        AlarmSound = x.AlarmLevelDefine.AlarmSound,
                        IsFlashing = x.AlarmLevelDefine.IsFlashing,
                        IsToSCADA = x.AlarmLevelDefine.IsToSCADA
                    }
                })
            );
            RaisePropertyChanged(nameof(AlarmTypeDefines));

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
            if (_originalAlarmTypeDefines == null || AlarmTypeDefines == null)
                return false;

            if (_originalAlarmTypeDefines.Count != AlarmTypeDefines.Count)
                return true;

            for (int i = 0; i < AlarmTypeDefines.Count; i++)
            {
                var curr = AlarmTypeDefines[i];
                var orig = _originalAlarmTypeDefines[i];
                if (curr.EquipAlarmTypeID != orig.EquipAlarmTypeID
                    || curr.EquipType != orig.EquipType
                    || curr.Description != orig.Description
                    || curr.DescriptionENG != orig.DescriptionENG
                    || curr.PossibleReason != orig.PossibleReason
                    || curr.PossibleReasonENG != orig.PossibleReasonENG
                    // AlarmLevelDefine 需比對內容
                    || curr.AlarmLevelDefine?.AlarmLevel != orig.AlarmLevelDefine?.AlarmLevel
                    || curr.AlarmLevelDefine?.IsNeedConfirm != orig.AlarmLevelDefine?.IsNeedConfirm
                    || curr.AlarmLevelDefine?.Color != orig.AlarmLevelDefine?.Color
                    || curr.AlarmLevelDefine?.AlarmSound != orig.AlarmLevelDefine?.AlarmSound
                    || curr.AlarmLevelDefine?.IsFlashing != orig.AlarmLevelDefine?.IsFlashing
                    || curr.AlarmLevelDefine?.IsToSCADA != orig.AlarmLevelDefine?.IsToSCADA
                )
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalAlarmTypeDefines = AlarmTypeDefines
                .Select(x => new EquipAlarmTypeDefineDto
                {
                    EquipAlarmTypeID = x.EquipAlarmTypeID,
                    EquipType = x.EquipType,
                    Description = x.Description,
                    DescriptionENG = x.DescriptionENG,
                    PossibleReason = x.PossibleReason,
                    PossibleReasonENG = x.PossibleReasonENG,
                    AlarmLevelDefine = x.AlarmLevelDefine == null ? null : new AlarmLevelDefineDto
                    {
                        AlarmLevel = x.AlarmLevelDefine.AlarmLevel,
                        IsNeedConfirm = x.AlarmLevelDefine.IsNeedConfirm,
                        Color = x.AlarmLevelDefine.Color,
                        AlarmSound = x.AlarmLevelDefine.AlarmSound,
                        IsFlashing = x.AlarmLevelDefine.IsFlashing,
                        IsToSCADA = x.AlarmLevelDefine.IsToSCADA
                    }
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            AlarmLevelDefines = new ObservableCollection<AlarmLevelDefineDto>(DesignTimeDatas.AlarmLevelDefines);
            AlarmTypeDefines = new ObservableCollection<EquipAlarmTypeDefineDto>(DesignTimeDatas.EquipAlarmTypeDefines);
            SelectedItem = DesignTimeDatas.EquipAlarmTypeDefines.FirstOrDefault();

            UpdateSnapshot();
        }
        #endregion
    }
}
