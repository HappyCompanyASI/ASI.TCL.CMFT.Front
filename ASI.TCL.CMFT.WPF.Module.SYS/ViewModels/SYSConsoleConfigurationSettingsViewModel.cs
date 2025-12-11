using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.SYS.Datas;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSConsoleConfigurationSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;
        

        #region Constructors
        public SYSConsoleConfigurationSettingsViewModel()
        {
        }
        public SYSConsoleConfigurationSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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
        public bool IsItemOperation => SelectedItem != null;

        private List<SYSConsoleDto> _originalConsoles;

        private SYSConsoleDto _selectedItem;
        public SYSConsoleDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        // 主控台配置資料
        private ObservableCollection<SYSConsoleDto> _sysConsoles = new();
        public ObservableCollection<SYSConsoleDto> SYSConsoles
        {
            get => _sysConsoles;
            set => SetProperty(ref _sysConsoles, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {
            Guid guid = Guid.NewGuid();

            //畫面
            var newItem = new SYSConsoleDto
            {
                Id = guid,
                SystemID = "SystemID",
                ConsoleName = "ConsoleName",
                IPAddress = "IPAddress",
                SetupLocation = "SetupLocation",
                SeatName = "SeatName",
                DLTNumber = "DLTNumber",
                TetraNumber = "TetraNumber"
            };

            //資料庫

            SYSConsoles.Add(newItem);
            UpdateSnapshot();
            SelectedItem = newItem;
        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {
            var itemToDelete = SelectedItem;

            if (itemToDelete == null)
                return;

            _dialogService.IsDelete($"確定要刪除\"{itemToDelete.ConsoleName}\"這筆設定嗎？", result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //資料庫

                    SelectedItem = null;

                    SYSConsoles.Remove(itemToDelete);

                    UpdateSnapshot();
                }
            }, "MainDialogHost");

        }
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
            SYSConsoles = new ObservableCollection<SYSConsoleDto>(
                _originalConsoles.Select(orig => new SYSConsoleDto
                {
                    Id = orig.Id,
                    SystemID = orig.SystemID,
                    ConsoleName = orig.ConsoleName,
                    IPAddress = orig.IPAddress,
                    SetupLocation = orig.SetupLocation,
                    SeatName = orig.SeatName,
                    DLTNumber = orig.DLTNumber,
                    TetraNumber = orig.TetraNumber
                })
            );
            RaisePropertyChanged(nameof(SYSConsoles));

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
            if (_originalConsoles == null || SYSConsoles == null)
                return false;

            if (_originalConsoles.Count != SYSConsoles.Count)
                return true;

            for (int i = 0; i < SYSConsoles.Count; i++)
            {
                var curr = SYSConsoles[i];
                var orig = _originalConsoles[i];
                if (curr.Id != orig.Id
                    || curr.SystemID != orig.SystemID
                    || curr.ConsoleName != orig.ConsoleName
                    || curr.IPAddress != orig.IPAddress
                    || curr.SetupLocation != orig.SetupLocation
                    || curr.SeatName != orig.SeatName
                    || curr.DLTNumber != orig.DLTNumber
                    || curr.TetraNumber != orig.TetraNumber)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalConsoles = SYSConsoles
                .Select(x => new SYSConsoleDto
                {
                    Id = x.Id,
                    SystemID = x.SystemID,
                    ConsoleName = x.ConsoleName,
                    IPAddress = x.IPAddress,
                    SetupLocation = x.SetupLocation,
                    SeatName = x.SeatName,
                    DLTNumber = x.DLTNumber,
                    TetraNumber = x.TetraNumber
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            SYSConsoles = new ObservableCollection<SYSConsoleDto>(DesignTimeDatas.SYSConsoles);
            SelectedItem = SYSConsoles.FirstOrDefault();
            UpdateSnapshot();
        }
        #endregion
    }
}
