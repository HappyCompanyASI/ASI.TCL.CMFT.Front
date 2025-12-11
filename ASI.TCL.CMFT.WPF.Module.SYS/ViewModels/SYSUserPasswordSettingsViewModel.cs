using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSUserPasswordSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSUserPasswordSettingsViewModel()
        {
        }
        public SYSUserPasswordSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
        }
        #endregion

        #region Properties
        private string _oldPassWord;
        public string OldPassWord
        {
            get => _oldPassWord; 
            set => SetProperty(ref _oldPassWord, value);
        }

        private string _newPassWord;
        public string NewPassWord
        {
            get => _newPassWord;
            set => SetProperty(ref _newPassWord, value);
        }

        private string _confirmPassWord;
        public string ConfirmPassWord
        {
            get => _confirmPassWord;
            set => SetProperty(ref _confirmPassWord, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {

        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {

        }
        #endregion
    }
}
