using System;
using System.Windows.Media;
using ASI.TCL.CMFT.WPF.Utilities;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Dialogs
{
    public class MessageBoxViewModel : BindableBase, IDialogAware
    {
        #region Constructors
        public MessageBoxViewModel()
        {
            MessageBoxIcon = PackIconKind.EmoticonDevilOutline;
            MessageBoxColor = "#F05E1C".HexToBrush();
            Title = "Dialog Title";
            Message = "This is error message with TextWrapping";
            DialogFooterButton = eDialogFooterButtons.OKCancel;
        }
        #endregion

        #region IDialogAware
        private string _title = "error";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogOpened(IDialogParameters parameters)
        {
            MessageBoxIcon = parameters.GetValue<PackIconKind>("messageBoxIcons");
            MessageBoxColor = parameters.GetValue<string>("messageBoxColor").HexToBrush();
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
            DialogFooterButton = parameters.GetValue<eDialogFooterButtons>("dialogFooterButtons");
        }
        public void OnDialogClosed() { }
        #endregion

        #region Properties
        private PackIconKind _messageBoxIcon;
        public PackIconKind MessageBoxIcon
        {
            get => _messageBoxIcon;
            set => SetProperty(ref _messageBoxIcon, value);
        }
        private SolidColorBrush _messageBoxColor;
        public SolidColorBrush MessageBoxColor
        {
            get => _messageBoxColor;
            set => SetProperty(ref _messageBoxColor, value);
        }
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        private eDialogFooterButtons _dialogFooterButton;
        public eDialogFooterButtons DialogFooterButton
        {
            get => _dialogFooterButton;
            set => SetProperty(ref _dialogFooterButton, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand<ButtonResult?> _buttonCommand;
        public DelegateCommand<ButtonResult?> ButtonCommand => _buttonCommand ??= new DelegateCommand<ButtonResult?>(OnExecute);
        private void OnExecute(ButtonResult? buttonResult)
        {
            if (buttonResult is { } Result && CanCloseDialog())
            {
                RequestClose?.Invoke(new DialogResult(Result, null));
            }
        }
        #endregion
    }
}