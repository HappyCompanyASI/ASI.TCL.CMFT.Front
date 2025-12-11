using System.Collections.ObjectModel;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.DMD.ViewModels
{
    public class DMDStationViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        #region Constructors
        public DMDStationViewModel()
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

        private eMessageDirection _selectedDirection = eMessageDirection.Message;
        public eMessageDirection SelectedDirection
        {
            get => _selectedDirection;
            set => SetProperty(ref _selectedDirection, value);
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex; 
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private DMDPreRecordMessageDto _selectedMessage;
        public DMDPreRecordMessageDto SelectedMessage
        {
            get => _selectedMessage; 
            set => SetProperty(ref _selectedMessage, value);
        }

        /// <summary>
        /// 播放類型
        /// </summary>
        private eMessageType _selectedType = eMessageType.Government;
        public eMessageType SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }

        /// <summary>
        /// 撥放次數
        /// </summary>
        private int _playCount = 1;
        public int PlayCount
        {
            get => _playCount;
            set => SetProperty(ref _playCount, value);
        }

        /// <summary>
        /// 即時訊息內容
        /// </summary>
        private string _instantMessage;
        public string InstantMessage
        {
            get => _instantMessage; 
            set => SetProperty(ref _instantMessage, value);
        }

        private ObservableCollection<object> _selectedTargets = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedTargets
        {
            get => _selectedTargets;
            set => SetProperty(ref _selectedTargets, value);
        }
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

        private DelegateCommand _deselectItemCommand;
        public DelegateCommand DseleteItemCommand => _deselectItemCommand ??= new DelegateCommand(ExcuteDseleteItemCommand);
        private void ExcuteDseleteItemCommand()
        {
            SelectedMessage = null;
        }
        #endregion


    }
}
