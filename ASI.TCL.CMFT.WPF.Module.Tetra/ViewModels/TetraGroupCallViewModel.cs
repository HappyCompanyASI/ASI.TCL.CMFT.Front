using System.Collections.ObjectModel;
using ASI.TCL.CMFT.WPF.Module.Tetra.DataTypes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.ViewModels
{
    public class TetraGroupCallViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        #region Constructors
        public TetraGroupCallViewModel()
        {

        }
        #endregion

        #region Properties
        private eStep? _currentStep = eStep.Step1;
        public eStep? CurrentStep
        {
            get => _currentStep;
            set => SetProperty(ref _currentStep, value);
        }
        public ObservableCollection<object> SelectedTargets { get; set; } = new ObservableCollection<object>();
        #endregion

        #region DelegateCommands
        private DelegateCommand<object> _deleteCommand;
        public DelegateCommand<object> DeleteCommand => _deleteCommand ??= new DelegateCommand<object>(ExcuteDeleteCommand);
        private void ExcuteDeleteCommand(object domaonModel)
        {
            SelectedTargets.Remove(domaonModel);
        }

        private DelegateCommand<eStep?> _stepChangeCommand;
        public DelegateCommand<eStep?> StepChangeCommand => _stepChangeCommand ??= new DelegateCommand<eStep?>(ExcuteStepChangeCommand);
        private void ExcuteStepChangeCommand(eStep? step)
        {
            CurrentStep = step;
        }
        #endregion

    }
}
