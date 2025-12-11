using System;
using ASI.TCL.CMFT.WPF.Applications;
using Prism.Commands;

namespace ASI.TCL.CMFT.WPF.Exceptions
{
    public class ExceptionWindowViewModel : ICloseWindow
    {
        #region Constructors
        public ExceptionWindowViewModel()
        {
        }
        public ExceptionWindowViewModel(string defaultMessage, string customMessage, string stackInfo)
        {
            vmpDefaultMessage = defaultMessage;
            vmpCustomMessage = customMessage;
            vmpStackInfo = stackInfo;
        }
        #endregion

        #region ICloseWindow
        public Action Close { get; set; }
        public bool? DialogResult { get; set; }
        public bool CanClose() => true;
        #endregion

        #region Properties
        public string Title { get; set; }
        public string vmpDefaultMessage { get; set; }
        public string vmpCustomMessage { get; set; }
        public string vmpStackInfo { get; set; }
        #endregion

        #region DelegateCommands
        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand => _okCommand ??= new DelegateCommand(ExcuteOKCommand);
        private void ExcuteOKCommand() => Close();
        #endregion
    }
}