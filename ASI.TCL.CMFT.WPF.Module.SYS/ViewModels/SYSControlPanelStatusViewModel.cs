using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.SYS.Datas;
using ASI.TCL.CMFT.WPF.Module.SYS.Dtos;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    internal class SYSControlPanelStatusViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public SYSControlPanelStatusViewModel()
        {
        }
        public SYSControlPanelStatusViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            
            _dialogService = dialogService;
            
            _eventAggregator = eventAggregator;

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private ObservableCollection<SYSConsoleStateDto> _sysconsoleStates = new();
        public ObservableCollection<SYSConsoleStateDto> SYSConsoleStates
        {
            get => _sysconsoleStates;
            set => SetProperty(ref _sysconsoleStates, value);
        }
        #endregion

        #region Private Methods
        private async Task InitDataAsync()
        {
            SYSConsoleStates = new ObservableCollection<SYSConsoleStateDto>(DesignTimeDatas.SYSConsoleStates);
        }
        #endregion

    }
}
