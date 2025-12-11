using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PAVoiceGroupSettingsDialogViewModel : BindableBase, IDialogAware
    {
        #region Constructors
        public PAVoiceGroupSettingsDialogViewModel()
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
        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
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
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                HintText = "群組名稱不能為空";
                return;
            }

            var param = new DialogParameters { { "GroupName", GroupName } };
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
