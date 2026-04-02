using Prism.Commands;
using Prism.Mvvm;

namespace ASI.TCL.CMFT.WPF.TestApp
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
        }

        private DelegateCommand _buttonCommand;
        public DelegateCommand ButtonCommand => _buttonCommand ??= new DelegateCommand(ExecuteButtonCommand);
        private void ExecuteButtonCommand()
        {
        }
    }
}
