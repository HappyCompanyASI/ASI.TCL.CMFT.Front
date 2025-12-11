using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.Alarm.Datas;
using ASI.TCL.CMFT.WPF.Module.Alarm.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.Alarm.ViewModels
{
    public class SystemAlarmMainViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;


        #region Constructors
        public SystemAlarmMainViewModel()
        {
        }
        public SystemAlarmMainViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            
            _dialogService = dialogService;
            
            _eventAggregator = eventAggregator;

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 臨時使用的集合，之後需要改成告警用的
        /// </summary>
        private ObservableCollection<SYSOperationLogDto> _sysoperationLogs = new();
        public ObservableCollection<SYSOperationLogDto> SYSOperationLogs
        {
            get => _sysoperationLogs;
            set => SetProperty(ref _sysoperationLogs, value);
        }

        private DateTime? _selectedStartTime;
        public DateTime? SelectedStartTime
        {
            get => _selectedStartTime;
            set => SetProperty(ref _selectedStartTime, value);
        }

        private DateTime? _selectedEndTime;
        public DateTime? SelectedEndTime
        {
            get => _selectedEndTime;
            set => SetProperty(ref _selectedEndTime, value);
        }

        private string _selectedSystemTypes;
        public string SelectedSystemTypes
        {
            get => _selectedSystemTypes;
            set => SetProperty(ref _selectedSystemTypes, value);
        }

        private SYSConsoleDto _selectedSYSConsole;
        public SYSConsoleDto SelectedSYSConsole
        {
            get => _selectedSYSConsole;
            set => SetProperty(ref _selectedSYSConsole, value);
        }

        private string _selectedAccountId;
        public string SelectedAccountId
        {
            get => _selectedAccountId;
            set => SetProperty(ref _selectedAccountId, value);
        }

        private string _selectedContent;
        public string SelectedContent
        {
            get => _selectedContent;
            set => SetProperty(ref _selectedContent, value);
        }

        private string _selectedAlarmCode;
        public string SelectedAlarmCode
        {
            get => _selectedAlarmCode;
            set => SetProperty(ref _selectedAlarmCode, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(ExcuteSearchCommand);
        private void ExcuteSearchCommand()
        {

        }

        private DelegateCommand _remitCommand;
        public DelegateCommand RemitCommand => _remitCommand ??= new DelegateCommand(ExcuteRemitCommand);
        private void ExcuteRemitCommand()
        {

        }
        #endregion

        #region Private Methods
        private async Task InitDataAsync()
        {
            /// <summary>
            /// 臨時使用的集合，之後需要改成告警用的
            /// </summary>
            SYSOperationLogs = new ObservableCollection<SYSOperationLogDto>(DesignTimeDatas.SYSOperationLogs);
        }
        #endregion
    }
}
