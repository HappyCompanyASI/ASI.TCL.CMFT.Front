using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.SYS.Datas;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSRegionalResponsibilitiesSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSRegionalResponsibilitiesSettingsViewModel()
        {
        }
        public SYSRegionalResponsibilitiesSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
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

        private List<SYSOTCSAuthorityDto> _originalOTCSAuthoritys;

        private SYSOTCSAuthorityDto _selectedItem;
        public SYSOTCSAuthorityDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        // 區域權責資料
        private ObservableCollection<SYSOTCSAuthorityDto> _sysOTCSAuthoritys = new();
        public ObservableCollection<SYSOTCSAuthorityDto> SYSOTCSAuthoritys
        {
            get => _sysOTCSAuthoritys;
            set => SetProperty(ref _sysOTCSAuthoritys, value);
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

            // 從快照還原（深複製）
            SYSOTCSAuthoritys = new ObservableCollection<SYSOTCSAuthorityDto>(
                _originalOTCSAuthoritys.Select(x => new SYSOTCSAuthorityDto
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
                    Authorities = new ObservableCollection<SYSAuthorityItemDto>(
                        x.Authorities.Select(a => new SYSAuthorityItemDto { IsChecked = a.IsChecked })
                    )
                })
            );
            RaisePropertyChanged(nameof(SYSOTCSAuthoritys));

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
            if (_originalOTCSAuthoritys == null || SYSOTCSAuthoritys == null)
                return false;

            if (_originalOTCSAuthoritys.Count != SYSOTCSAuthoritys.Count)
                return true;

            for (int i = 0; i < SYSOTCSAuthoritys.Count; i++)
            {
                var curr = SYSOTCSAuthoritys[i];
                var orig = _originalOTCSAuthoritys[i];
                if (curr.Console.Id != orig.Console.Id
                    || curr.Console.SystemID != orig.Console.SystemID
                    || curr.Console.ConsoleName != orig.Console.ConsoleName
                    || curr.Console.IPAddress != orig.Console.IPAddress
                    || curr.Console.SetupLocation != orig.Console.SetupLocation
                    || curr.Console.SeatName != orig.Console.SeatName
                    || curr.Console.DLTNumber != orig.Console.DLTNumber
                    || curr.Console.TetraNumber != orig.Console.TetraNumber
                    || curr.Authorities.Count != orig.Authorities.Count
                    || !curr.Authorities.Select(a => a.IsChecked).SequenceEqual(orig.Authorities.Select(a => a.IsChecked)))
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalOTCSAuthoritys = SYSOTCSAuthoritys
                .Select(x => new SYSOTCSAuthorityDto
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
                    Authorities = x.Authorities != null
                        ? x.Authorities.Select(a => new SYSAuthorityItemDto { IsChecked = a.IsChecked }).ToList()
                        : new List<SYSAuthorityItemDto>()
                })
                .ToList();
        }
        private async Task InitDataAsync()
        {
            SYSOTCSAuthoritys = new ObservableCollection<SYSOTCSAuthorityDto>(
                DesignTimeDatas.SYSOTCSAuthoritys.Select(dto =>
                {
                    dto.Authorities = new ObservableCollection<SYSAuthorityItemDto>(dto.Authorities ?? new List<SYSAuthorityItemDto>());
                    return dto;
                })
            );

            SelectedItem = SYSOTCSAuthoritys.FirstOrDefault();
            UpdateSnapshot();
        }
        #endregion
    }
}
