using System.Collections.ObjectModel;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PAMainViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
   
        #region Constructors
        public PAMainViewModel()
        {
        }
        public PAMainViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
   
            //InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private ObservableCollection<object> _currentTrainsPlaying = new ObservableCollection<object>();
        public ObservableCollection<object> CurrentTrainsPlaying
        {
            get => _currentTrainsPlaying;
            set => SetProperty(ref _currentTrainsPlaying, value);
        }

        private ObservableCollection<object> _currentStationsPlaying = new ObservableCollection<object>();
        public ObservableCollection<object> CurrentStationsPlaying
        {
            get => _currentStationsPlaying;
            set => SetProperty(ref _currentStationsPlaying, value);
        }
        #endregion


    }
}
