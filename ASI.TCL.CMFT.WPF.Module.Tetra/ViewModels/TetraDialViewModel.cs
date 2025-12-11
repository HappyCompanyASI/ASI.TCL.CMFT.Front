using System.Collections.ObjectModel;
using ASI.TCL.CMFT.WPF.Module.Tetra.DataTypes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.ViewModels
{
    public class TetraDialViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        #region Constructors
        public TetraDialViewModel()
        {
            // Initialize DialNumber with default value to match XAML
            DialNumber = "61076";
        }
        #endregion

        #region Properties
        private eStep? _currentStep = eStep.Step1;
        public eStep? CurrentStep
        {
            get => _currentStep;
            set => SetProperty(ref _currentStep, value);
        }

        private string _dialNumber;
        public string DialNumber
        {
            get => _dialNumber;
            set => SetProperty(ref _dialNumber, value);
        }

        private int _maxDialNumberLength = 10;
        public int MaxDialNumberLength
        {
            get => _maxDialNumberLength;
        }

        public ObservableCollection<object> SelectedTargets { get; set; } = new ObservableCollection<object>();
        #endregion

        #region DelegateCommands
        private DelegateCommand<eStep?> _stepChangeCommand;
        public DelegateCommand<eStep?> StepChangeCommand => _stepChangeCommand ??= new DelegateCommand<eStep?>(ExcuteStepChangeCommand);
        private void ExcuteStepChangeCommand(eStep? step)
        {
            CurrentStep = step;
        }

        private DelegateCommand<string> _numberButtonCommand;
        public DelegateCommand<string> NumberButtonCommand => _numberButtonCommand ??= new DelegateCommand<string>(ExecuteNumberButtonCommand);
        private void ExecuteNumberButtonCommand(string number)
        {
            if (!string.IsNullOrEmpty(number) && (DialNumber?.Length ?? 0) < MaxDialNumberLength)
            {
                DialNumber += number;
            }
        }

        private DelegateCommand _backspaceCommand;
        public DelegateCommand BackspaceCommand => _backspaceCommand ??= new DelegateCommand(ExecuteBackspaceCommand);
        private void ExecuteBackspaceCommand()
        {
            if (!string.IsNullOrEmpty(DialNumber) && DialNumber.Length > 0)
            {
                DialNumber = DialNumber.Substring(0, DialNumber.Length - 1);
            }
        }

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand => _clearCommand ??= new DelegateCommand(ExecuteClearCommand);
        private void ExecuteClearCommand()
        {
            DialNumber = string.Empty;
        }
        #endregion
    }
}
