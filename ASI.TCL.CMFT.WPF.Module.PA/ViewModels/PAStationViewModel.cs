using System.Collections.ObjectModel;
using ASI.TCL.CMFT.WPF.Module.PA.DataTypes;
using ASI.TCL.CMFT.WPF.Module.PA.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PAStationViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        #region Constructors
        public PAStationViewModel()
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

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        private PAPreRecordVoiceDto _selectedVoice;
        public PAPreRecordVoiceDto SelectedVoice
        {
            get => _selectedVoice;
            set => SetProperty(ref _selectedVoice, value);
        }

        /// <summary>
        /// 無限撥放
        /// </summary>
        private bool _isInfinitePlayback;
        public bool IsInfinitePlayback
        {
            get => _isInfinitePlayback;
            set => SetProperty(ref _isInfinitePlayback, value);
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
        /// 使用者選擇播放國語
        /// </summary>
        private bool _isChineseSelected = true;
        public bool IsChineseSelected
        {
            get => _isChineseSelected;
            set => SetProperty(ref _isChineseSelected, value);
        }

        /// <summary>
        /// 使用者選擇播放台語
        /// </summary>
        private bool _isTaiwaneseSelected = true;
        public bool IsTaiwaneseSelected
        {
            get => _isTaiwaneseSelected;
            set => SetProperty(ref _isTaiwaneseSelected, value);
        }

        /// <summary>
        /// 使用者選擇播放客語
        /// </summary>
        private bool _isHakkaSelected = true;
        public bool IsHakkaSelected
        {
            get => _isHakkaSelected;
            set => SetProperty(ref _isHakkaSelected, value);
        }

        /// <summary>
        /// 使用者選擇播放英語
        /// </summary>
        private bool _isEnglishSelected = true;
        public bool IsEnglishSelected
        {
            get => _isEnglishSelected;
            set => SetProperty(ref _isEnglishSelected, value);
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
            SelectedVoice = null;
        }
        #endregion
    }
}
