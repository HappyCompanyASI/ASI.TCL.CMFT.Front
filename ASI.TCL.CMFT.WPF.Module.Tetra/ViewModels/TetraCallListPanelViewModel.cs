using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.Tetra.Datas;
using ASI.TCL.CMFT.WPF.Module.Tetra.Dtos;
using ASI.TCL.CMFT.WPF.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.ViewModels
{
    public class TetraCallListPanelViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public TetraCallListPanelViewModel()
        {
            TetraCallQueue = new ObservableCollection<TetraCallerDisplayDto>(SampleData.TetraCallerDisplay);
        }

        public TetraCallListPanelViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // 訂閱無線電來電通知事件
            _eventAggregator.GetEvent<TetraCallNotificationEvent>().Subscribe(OnTetraCallReceived);

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties

        private TetraCallerDisplayDto _selectedTetraItem;
        public TetraCallerDisplayDto SelectedTetraItem
        {
            get => _selectedTetraItem;
            set
            {
                SetProperty(ref _selectedTetraItem, value);
                // 觸發 Command 的 CanExecute 重新評估
                AnswerTetraCallCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<TetraCallerDisplayDto> _tetraCallQueue = new();
        public ObservableCollection<TetraCallerDisplayDto> TetraCallQueue
        {
            get => _tetraCallQueue;
            set => SetProperty(ref _tetraCallQueue, value);
        }
        #endregion

        #region DelegateCommands
        // 無線電接聽 Command
        private DelegateCommand _answerTetraCallCommand;
        public DelegateCommand AnswerTetraCallCommand => _answerTetraCallCommand ??= new DelegateCommand(ExecuteAnswerTetraCallCommand, CanExecuteAnswerTetraCallCommand);

        private bool CanExecuteAnswerTetraCallCommand()
        {
            return SelectedTetraItem != null;
        }

        private void ExecuteAnswerTetraCallCommand()
        {
            if (SelectedTetraItem == null) return;

            Debug.WriteLine($"接聽無線電來電: {SelectedTetraItem.PhoneNumber} @ {SelectedTetraItem.BaseStation} ({SelectedTetraItem.TetraCallType})");

            // 發送接聽事件通知到 TetraLiveCallPanelViewModel
            var answerEventArgs = new TetraCallAnsweredEventArgs(
                callId: SelectedTetraItem.Id ?? System.Guid.NewGuid().ToString(),
                phoneNumber: SelectedTetraItem.PhoneNumber ?? "",
                baseStation: SelectedTetraItem.BaseStation ?? "",
                callType: SelectedTetraItem.TetraCallType.ToString()
            );
            _eventAggregator.GetEvent<TetraCallAnsweredEvent>().Publish(answerEventArgs);

            // 從列表中移除已接聽的來電
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                TetraCallQueue.Remove(SelectedTetraItem);
                SelectedTetraItem = null;
            });
        }
        #endregion

        #region Event Handlers
        private void OnTetraCallReceived(TetraCallNotificationEventArgs args)
        {
            Debug.WriteLine($"TetraCallListPanelViewModel 收到無線電來電通知: {args.PhoneNumber} @ {args.BaseStation} ({args.CallType})");

            // 創建新的無線電來電項目
            var newCall = new TetraCallerDisplayDto
            {
                Id = System.Guid.NewGuid().ToString(),
                PhoneNumber = args.PhoneNumber,
                BaseStation = args.BaseStation,
                TetraCallType = System.Enum.TryParse<eTetraCallType>(args.CallType, out var callType) ? callType : eTetraCallType.一般
            };

            // 添加到無線電來電佇列
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                TetraCallQueue.Add(newCall);
            });
        }
        #endregion

        #region PrivateMethods
        private async Task InitDataAsync()
        {
            TetraCallQueue = new ObservableCollection<TetraCallerDisplayDto>(SampleData.TetraCallerDisplay);
        }
        #endregion
    }
}
