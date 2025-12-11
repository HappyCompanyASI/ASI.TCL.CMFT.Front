using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ASI.TCL.CMFT.WPF.Events;
using ASI.TCL.CMFT.WPF.Module.Tetra.Datas;
using ASI.TCL.CMFT.WPF.Module.Tetra.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.ViewModels
{
    public class TetraLiveCallPanelViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        #region Constructors
        public TetraLiveCallPanelViewModel()
        {
            // 設計時資料
            ChannelList = new ObservableCollection<RadioContentDto>(DesignTimeDatas.RadioContent);
        }
        
        public TetraLiveCallPanelViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // 訂閱 OTCS 來電接聽事件
            _eventAggregator.GetEvent<PICallAnsweredEvent>().Subscribe(OnPICallAnswered);
            _eventAggregator.GetEvent<SICallAnsweredEvent>().Subscribe(OnSICallAnswered);

            // 訂閱 Tetra 來電接聽事件
            _eventAggregator.GetEvent<TetraCallAnsweredEvent>().Subscribe(OnTetraCallAnswered);

            //測試時資料
            ChannelList = new ObservableCollection<RadioContentDto>(DesignTimeDatas.RadioContent);

        }
        #endregion

        #region Properties
        private string _currentCallInfo;
        public string CurrentCallInfo
        {
            get => _currentCallInfo;
            set => SetProperty(ref _currentCallInfo, value);
        }

        private bool _isInCall;
        public bool IsInCall
        {
            get => _isInCall;
            set => SetProperty(ref _isInCall, value);
        }

        private ObservableCollection<RadioContentDto> _channelList = new ();
        public ObservableCollection<RadioContentDto> ChannelList
        {
            get => _channelList;
            set => SetProperty(ref _channelList, value);
        }

        private RadioContentDto _selectedChannel;
        public RadioContentDto SelectedChannel
        {
            get => _selectedChannel;
            set => SetProperty(ref _selectedChannel, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _endCallCommand;
        public DelegateCommand EndCallCommand => _endCallCommand ??= new DelegateCommand(ExecuteEndCallCommand);
        private void ExecuteEndCallCommand()
        {
            var channelToEnd = SelectedChannel;
            if (channelToEnd == null)
                return;

            ChannelList.Remove(channelToEnd);

            Debug.WriteLine($"結束通話: {channelToEnd.ChannelName}");

        }
        #endregion


        #region Event Handlers
        private void OnPICallAnswered(PICallAnsweredEventArgs args)
        {
            Debug.WriteLine($"TetraLiveCallPanelViewModel 收到 PI 接聽通知: {args.TrainNumber}-{args.Car} PI#{args.PINumber}");
            
            // 處理 PI 來電接聽
            CurrentCallInfo = $"通話中 - PI: {args.TrainNumber}-{args.Car} PI#{args.PINumber} @ {args.BaseStation}";
            IsInCall = true;

            // TODO: 在此處實現實際的通話處理邏輯
            // 例如：建立音訊連線、更新UI狀態等

            // 模擬建立新的通話頻道
            var newChannel = new RadioContentDto
            {
                ChannelName = $"PI通話-{args.TrainNumber}",
                ChannelNumber = $"PI-{args.CallId}",
                Contents = new List<RadioContentItemDto>
                {
                    new RadioContentItemDto
                    {
                        NumberOrTrain = args.TrainNumber,
                        Detail = $"{args.Car}車,{args.PINumber}號PI @ {args.BaseStation}"
                    }
                }
            };

            // 檢查是否已存在相同的頻道，若沒有則新增
            var existingChannel = ChannelList.FirstOrDefault(c => c.ChannelNumber == newChannel.ChannelNumber);
            if (existingChannel == null)
            {
                ChannelList.Add(newChannel);
                Debug.WriteLine($"新增 PI 通話頻道: {newChannel.ChannelName} ({newChannel.ChannelNumber})");
            }
            else
            {
                // 如果頻道已存在，更新內容
                existingChannel.Contents.Clear();
                existingChannel.Contents.Add(newChannel.Contents.First());
                Debug.WriteLine($"更新現有 PI 通話頻道: {existingChannel.ChannelName}");
            }

            // 自動選擇新建立的頻道
            SelectedChannel = ChannelList.FirstOrDefault(c => c.ChannelNumber == newChannel.ChannelNumber);
        }

        private void OnSICallAnswered(SICallAnsweredEventArgs args)
        {
            Debug.WriteLine($"TetraLiveCallPanelViewModel 收到 SI 接聽通知: {args.TrainNumber}-{args.Car}");
            
            // 處理 SI 來電接聽
            CurrentCallInfo = $"通話中 - SI: {args.TrainNumber}-{args.Car} @ {args.BaseStation}";
            IsInCall = true;

            // TODO: 在此處實現實際的通話處理邏輯
            // 例如：建立音訊連線、更新UI狀態等

            // 模擬建立新的通話頻道
            var newChannel = new RadioContentDto
            {
                ChannelName = $"SI通話-{args.TrainNumber}",
                ChannelNumber = $"SI-{args.CallId}",
                Contents = new List<RadioContentItemDto>
                {
                    new RadioContentItemDto
                    {
                        NumberOrTrain = args.TrainNumber,
                        Detail = $"{args.Car}車 @ {args.BaseStation}"
                    }
                }
            };

            // 檢查是否已存在相同的頻道，若沒有則新增
            var existingChannel = ChannelList.FirstOrDefault(c => c.ChannelNumber == newChannel.ChannelNumber);
            if (existingChannel == null)
            {
                ChannelList.Add(newChannel);
                Debug.WriteLine($"新增 SI 通話頻道: {newChannel.ChannelName} ({newChannel.ChannelNumber})");
            }
            else
            {
                // 如果頻道已存在，更新內容
                existingChannel.Contents.Clear();
                existingChannel.Contents.Add(newChannel.Contents.First());
                Debug.WriteLine($"更新現有 SI 通話頻道: {existingChannel.ChannelName}");
            }

            // 自動選擇新建立的頻道
            SelectedChannel = ChannelList.FirstOrDefault(c => c.ChannelNumber == newChannel.ChannelNumber);
        }

        private void OnTetraCallAnswered(TetraCallAnsweredEventArgs args)
        {
            Debug.WriteLine($"TetraLiveCallPanelViewModel 收到無線電接聽通知: {args.PhoneNumber} @ {args.BaseStation} ({args.CallType})");
            
            // 處理無線電來電接聽
            CurrentCallInfo = $"通話中 - 無線電: {args.PhoneNumber} @ {args.BaseStation} ({args.CallType})";
            IsInCall = true;

            // TODO: 在此處實現實際的通話處理邏輯
            // 例如：建立音訊連線、更新UI狀態等

            // 模擬建立新的通話頻道
            var newChannel = new RadioContentDto
            {
                ChannelName = $"無線電通話-{args.PhoneNumber}",
                ChannelNumber = $"TETRA-{args.CallId}",
                Contents = new List<RadioContentItemDto>
                {
                    new RadioContentItemDto
                    {
                        NumberOrTrain = args.PhoneNumber,
                        Detail = $"{args.CallType} 呼叫 @ {args.BaseStation}"
                    }
                }
            };

            // 檢查是否已存在相同的頻道，若沒有則新增
            var existingChannel = ChannelList.FirstOrDefault(c => c.ChannelNumber == newChannel.ChannelNumber);
            if (existingChannel == null)
            {
                ChannelList.Add(newChannel);
                Debug.WriteLine($"新增無線電通話頻道: {newChannel.ChannelName} ({newChannel.ChannelNumber})");
            }
            else
            {
                // 如果頻道已存在，更新內容
                existingChannel.Contents.Clear();
                existingChannel.Contents.Add(newChannel.Contents.First());
                Debug.WriteLine($"更新現有無線電通話頻道: {existingChannel.ChannelName}");
            }

            // 自動選擇新建立的頻道
            SelectedChannel = ChannelList.FirstOrDefault(c => c.ChannelNumber == newChannel.ChannelNumber);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
