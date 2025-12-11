using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSUserAccountSettingsDialogViewModel : BindableBase, IDialogAware
    {
        #region Constructors
        public SYSUserAccountSettingsDialogViewModel()
        {
        }
        #endregion

        #region IDialogAware
        public string Title { get; }
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters) { }
        #endregion

        #region Properties
        private string _accountID;
        public string AccountID
        {
            get => _accountID;
            set => SetProperty(ref _accountID, value);
        }

        private string _hintText;
        public string HintText
        {
            get => _hintText;
            set => SetProperty(ref _hintText, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand => _okCommand ??= new DelegateCommand(ExcuteOKCommand);
        private void ExcuteOKCommand()
        {
            if (string.IsNullOrWhiteSpace(AccountID))
            {
                HintText = "職員編號不能為空";
                return;
            }

            var param = new DialogParameters { { "AccountID", AccountID } };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
        #endregion
    }
}
