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
    public class PABroadcastEquipmentSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public PABroadcastEquipmentSettingsViewModel()
        {
        }
        public PABroadcastEquipmentSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<PABroadcastEquipmentDto> _originalEquipments;

        //public bool IsItemOperation => SelectedItem != null;

        private PABroadcastEquipmentDto _selectedItem;
        public PABroadcastEquipmentDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                //RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        //廣播設備資料
        private ObservableCollection<PABroadcastEquipmentDto> _paStationBroadcastEquipments = new();
        public ObservableCollection<PABroadcastEquipmentDto> PAStationBroadcastEquipments
        {
            get => _paStationBroadcastEquipments;
            set => SetProperty(ref _paStationBroadcastEquipments, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedItem;

            //資料庫

            UpdateSnapshot();

            SelectedItem = null;
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

            // 從快照還原
            PAStationBroadcastEquipments = new ObservableCollection<PABroadcastEquipmentDto>(
                _originalEquipments.Select(x => new PABroadcastEquipmentDto
                {
                    Id = x.Id,
                    ConsoleName = x.ConsoleName,
                    Location = x.Location,
                    SeatName = x.SeatName,
                    IsStationBroadcastEnabled = x.IsStationBroadcastEnabled
                })
            );
            RaisePropertyChanged(nameof(PAStationBroadcastEquipments));

            SelectedItem = null;
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
            if (_originalEquipments == null || PAStationBroadcastEquipments == null)
                return false;

            if (_originalEquipments.Count != PAStationBroadcastEquipments.Count)
                return true;

            for (int i = 0; i < PAStationBroadcastEquipments.Count; i++)
            {
                var curr = PAStationBroadcastEquipments[i];
                var orig = _originalEquipments[i];
                if (curr.Id != orig.Id
                    || curr.ConsoleName != orig.ConsoleName
                    || curr.Location != orig.Location
                    || curr.SeatName != orig.SeatName
                    || curr.IsStationBroadcastEnabled != orig.IsStationBroadcastEnabled)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalEquipments = PAStationBroadcastEquipments
                .Select(x => new PABroadcastEquipmentDto
                {
                    Id = x.Id,
                    ConsoleName = x.ConsoleName,
                    Location = x.Location,
                    SeatName = x.SeatName,
                    IsStationBroadcastEnabled = x.IsStationBroadcastEnabled
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            PAStationBroadcastEquipments = new ObservableCollection<PABroadcastEquipmentDto>(DesignTimeDatas.PABroadcastEquipments);
            SelectedItem = PAStationBroadcastEquipments.FirstOrDefault();
            UpdateSnapshot();
        }
        #endregion
    }
}
