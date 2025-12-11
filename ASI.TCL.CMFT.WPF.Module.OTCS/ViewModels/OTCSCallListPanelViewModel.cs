using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Module.OTCS.Datas;
using ASI.TCL.CMFT.WPF.Module.OTCS.Dtos;
using ASI.TCL.CMFT.WPF.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ASI.TCL.CMFT.WPF.Module.OTCS.ViewModels
{
    public class OTCSCallListPanelViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public OTCSCallListPanelViewModel()
        {
            PICallQueue = new ObservableCollection<PICallerDisplayDto>(SampleData.PICallerDisplay);
            SICallQueue = new ObservableCollection<SICallerDisplayDto>(SampleData.SICallerDisplay);
        }

        public OTCSCallListPanelViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // 訂閱 PI 來電通知事件
            _eventAggregator.GetEvent<PICallNotificationEvent>().Subscribe(OnPICallReceived);
            
            // 訂閱 SI 來電通知事件
            _eventAggregator.GetEvent<SICallNotificationEvent>().Subscribe(OnSICallReceived);

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties

        private PICallerDisplayDto _selectedPIItem;
        public PICallerDisplayDto SelectedPIItem
        {
            get => _selectedPIItem; 
            set 
            { 
                SetProperty(ref _selectedPIItem, value);
                // 觸發 Command 的 CanExecute 重新評估
                AnswerPICallCommand.RaiseCanExecuteChanged();
            }
        }

        private SICallerDisplayDto _selectedSIItem;
        public SICallerDisplayDto SelectedSIItem
        {
            get => _selectedSIItem;
            set 
            { 
                SetProperty(ref _selectedSIItem, value);
                // 觸發 Command 的 CanExecute 重新評估
                AnswerSICallCommand.RaiseCanExecuteChanged();
            }
        }


        private ObservableCollection<PICallerDisplayDto> _piCallQueue = new();
        public ObservableCollection<PICallerDisplayDto> PICallQueue
        {
            get => _piCallQueue;
            set => SetProperty(ref _piCallQueue, value);
        }

        private ObservableCollection<SICallerDisplayDto> _siCallQueue = new();
        public ObservableCollection<SICallerDisplayDto> SICallQueue
        {
            get => _siCallQueue;
            set => SetProperty(ref _siCallQueue, value);
        }
        #endregion

        #region DelegateCommands
        // PI 接聽 Command
        private DelegateCommand _answerPICallCommand;
        public DelegateCommand AnswerPICallCommand => _answerPICallCommand ??= new DelegateCommand(ExecuteAnswerPICallCommand, CanExecuteAnswerPICallCommand);
        
        private bool CanExecuteAnswerPICallCommand()
        {
            return SelectedPIItem != null;
        }
        
        private void ExecuteAnswerPICallCommand()
        {
            if (SelectedPIItem == null) return;

            Debug.WriteLine($"接聽 PI 來電: {SelectedPIItem.Train?.TrainNumber}-{SelectedPIItem.Car} PI#{SelectedPIItem.PINumber}");
            
            // 發送接聽事件通知到 TetraLiveCallPanelViewModel
            var answerEventArgs = new PICallAnsweredEventArgs(
                callId: SelectedPIItem.Id ?? System.Guid.NewGuid().ToString(),
                trainNumber: SelectedPIItem.Train?.TrainNumber ?? "",
                car: SelectedPIItem.Car ?? "",
                piNumber: SelectedPIItem.PINumber ?? "",
                baseStation: SelectedPIItem.Source ?? ""
            );
            _eventAggregator.GetEvent<PICallAnsweredEvent>().Publish(answerEventArgs);
            
            // 從列表中移除已接聽的來電
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                PICallQueue.Remove(SelectedPIItem);
                SelectedPIItem = null;
            });
        }

        // SI 接聽 Command
        private DelegateCommand _answerSICallCommand;
        public DelegateCommand AnswerSICallCommand => _answerSICallCommand ??= new DelegateCommand(ExecuteAnswerSICallCommand, CanExecuteAnswerSICallCommand);
        
        private bool CanExecuteAnswerSICallCommand()
        {
            return SelectedSIItem != null;
        }
        
        private void ExecuteAnswerSICallCommand()
        {
            if (SelectedSIItem == null) return;

            Debug.WriteLine($"接聽 SI 來電: {SelectedSIItem.Train?.TrainNumber}-{SelectedSIItem.Car}");
            
            // 發送接聽事件通知到 TetraLiveCallPanelViewModel
            var answerEventArgs = new SICallAnsweredEventArgs(
                callId: SelectedSIItem.Id ?? System.Guid.NewGuid().ToString(),
                trainNumber: SelectedSIItem.Train?.TrainNumber ?? "",
                car: SelectedSIItem.Car ?? "",
                baseStation: SelectedSIItem.BaseStation ?? ""
            );
            _eventAggregator.GetEvent<SICallAnsweredEvent>().Publish(answerEventArgs);
            
            // 從列表中移除已接聽的來電
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                SICallQueue.Remove(SelectedSIItem);
                SelectedSIItem = null;
            });
        }
        #endregion

        #region Event Handlers
        private void OnPICallReceived(PICallNotificationEventArgs args)
        {
            Debug.WriteLine($"OTCSCallListPanelViewModel 收到 PI 來電通知: {args.TrainNumber}-{args.Car} PI#{args.PINumber}");
            
            // 創建新的 PI 來電項目
            var newCall = new PICallerDisplayDto
            {
                Id = System.Guid.NewGuid().ToString(),
                Train = new OTCSTarinDto
                {
                    TrainNumber = args.TrainNumber
                },
                Car = args.Car,
                PINumber = args.PINumber,
                Source = args.BaseStation
            };
            
            // 添加到 PI 來電佇列
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                PICallQueue.Add(newCall);
            });
        }

        private void OnSICallReceived(SICallNotificationEventArgs args)
        {
            Debug.WriteLine($"OTCSCallListPanelViewModel 收到 SI 來電通知: {args.TrainNumber}-{args.Car}");
            
            // 創建新的 SI 來電項目
            var newCall = new SICallerDisplayDto
            {
                Id = System.Guid.NewGuid().ToString(),
                Train = new OTCSTarinDto
                {
                    TrainNumber = args.TrainNumber
                },
                Car = args.Car,
                BaseStation = args.BaseStation
            };
            
            // 添加到 SI 來電佇列
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                SICallQueue.Add(newCall);
            });
        }
        #endregion

        #region PrivateMethods
        private async Task InitDataAsync()
        {
            PICallQueue = new ObservableCollection<PICallerDisplayDto>(SampleData.PICallerDisplay);
            SICallQueue = new ObservableCollection<SICallerDisplayDto>(SampleData.SICallerDisplay);
        }
        #endregion
    }
}