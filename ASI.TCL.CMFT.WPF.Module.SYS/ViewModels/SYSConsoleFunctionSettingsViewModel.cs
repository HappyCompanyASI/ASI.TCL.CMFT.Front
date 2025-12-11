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
    public class SYSConsoleFunctionSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSConsoleFunctionSettingsViewModel()
        {
        }
        public SYSConsoleFunctionSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<SYSCMFTAuthorityDto> _originalCMFTAuthoritys;

        private SYSCMFTAuthorityDto _selectedItem;
        public SYSCMFTAuthorityDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        // 主控台功能資料
        private ObservableCollection<SYSCMFTAuthorityDto> _sysCMFTAuthoritys = new();
        public ObservableCollection<SYSCMFTAuthorityDto> SYSCMFTAuthoritys
        {
            get => _sysCMFTAuthoritys;
            set => SetProperty(ref _sysCMFTAuthoritys, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private void ExcuteAddItemCommand()
        {
            Guid guid = Guid.NewGuid();

            //畫面
            var newItem = new SYSCMFTAuthorityDto
            {
                Console = new SYSConsoleDto
                {
                    Id = Guid.NewGuid(),
                    SystemID = "DefaultSystemID",
                    ConsoleName = "DefaultConsoleName",
                    IPAddress = "0.0.0.0",
                    SetupLocation = "DefaultLocation",
                    SeatName = "DefaultSeat",
                    DLTNumber = "0000",
                    TetraNumber = "0000"
                },
                IsDMDEnable = false,
                IsPAEnable = false,
                IsTetraEnable = false,
                IsOTCSEnable = false,
                IsSystemAlarmEnable = false,
                IsLogSearchEnable = false
            };
     
            //資料庫

            SYSCMFTAuthoritys.Add(newItem);
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

            _dialogService.IsDelete($"確定要刪除\"{itemToDelete.Console.ConsoleName}\"這筆設定嗎？", result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    //資料庫

                    SelectedItem = null;

                    SYSCMFTAuthoritys.Remove(itemToDelete);

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
            SYSCMFTAuthoritys = new ObservableCollection<SYSCMFTAuthorityDto>(
                _originalCMFTAuthoritys.Select(orig => new SYSCMFTAuthorityDto
                {
                    Console = new SYSConsoleDto
                    {
                        Id = orig.Console.Id,
                        SystemID = orig.Console.SystemID,
                        ConsoleName = orig.Console.ConsoleName,
                        IPAddress = orig.Console.IPAddress,
                        SetupLocation = orig.Console.SetupLocation,
                        SeatName = orig.Console.SeatName,
                        DLTNumber = orig.Console.DLTNumber,
                        TetraNumber = orig.Console.TetraNumber
                    },
                    IsDMDEnable = orig.IsDMDEnable,
                    IsPAEnable = orig.IsPAEnable,
                    IsTetraEnable = orig.IsTetraEnable,
                    IsOTCSEnable = orig.IsOTCSEnable,
                    IsSystemAlarmEnable = orig.IsSystemAlarmEnable,
                    IsLogSearchEnable = orig.IsLogSearchEnable
                })
            );
            RaisePropertyChanged(nameof(SYSCMFTAuthoritys));

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
            if (_originalCMFTAuthoritys == null || SYSCMFTAuthoritys == null)
                return false;

            if (_originalCMFTAuthoritys.Count != SYSCMFTAuthoritys.Count)
                return true;

            for (int i = 0; i < SYSCMFTAuthoritys.Count; i++)
            {
                var curr = SYSCMFTAuthoritys[i];
                var orig = _originalCMFTAuthoritys[i];
                if (curr.Console.Id != orig.Console.Id
                    || curr.Console.SystemID != orig.Console.SystemID
                    || curr.Console.ConsoleName != orig.Console.ConsoleName
                    || curr.Console.IPAddress != orig.Console.IPAddress
                    || curr.Console.SetupLocation != orig.Console.SetupLocation
                    || curr.Console.SeatName != orig.Console.SeatName
                    || curr.Console.DLTNumber != orig.Console.DLTNumber
                    || curr.Console.TetraNumber != orig.Console.TetraNumber
                    || curr.IsDMDEnable != orig.IsDMDEnable
                    || curr.IsPAEnable != orig.IsPAEnable
                    || curr.IsTetraEnable != orig.IsTetraEnable
                    || curr.IsOTCSEnable != orig.IsOTCSEnable
                    || curr.IsSystemAlarmEnable != orig.IsSystemAlarmEnable
                    || curr.IsLogSearchEnable != orig.IsLogSearchEnable)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalCMFTAuthoritys = SYSCMFTAuthoritys
                .Select(x => new SYSCMFTAuthorityDto
                {
                    Console = new SYSConsoleDto
                    {
                        Id = x.Console.Id,
                        SystemID = x.Console.SystemID,
                        ConsoleName = x.Console.ConsoleName,
                        IPAddress = x.Console.IPAddress,
                        SetupLocation = x.Console.SetupLocation,
                        SeatName = x.Console.SeatName,
                        DLTNumber = x.Console.DLTNumber,
                        TetraNumber = x.Console.TetraNumber
                    },
                    IsDMDEnable = x.IsDMDEnable,
                    IsPAEnable = x.IsPAEnable,
                    IsTetraEnable = x.IsTetraEnable,
                    IsOTCSEnable = x.IsOTCSEnable,
                    IsSystemAlarmEnable = x.IsSystemAlarmEnable,
                    IsLogSearchEnable = x.IsLogSearchEnable
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            SYSCMFTAuthoritys = new ObservableCollection<SYSCMFTAuthorityDto>(DesignTimeDatas.SYSCMFTAuthoritys);
            SelectedItem = SYSCMFTAuthoritys.FirstOrDefault();
            UpdateSnapshot();
        }
        #endregion
    }
}
