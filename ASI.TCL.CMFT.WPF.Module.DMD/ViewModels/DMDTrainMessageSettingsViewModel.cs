using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using ASI.TCL.CMFT.WPF.Module.DMD.Datas;
using ASI.TCL.CMFT.WPF.Module.DMD.DataTypes;
using ASI.TCL.CMFT.WPF.Module.DMD.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.DMD.ViewModels
{
    public class DMDTrainMessageSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;


        #region Constructors
        public DMDTrainMessageSettingsViewModel()
        {
        }
        public DMDTrainMessageSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        private DMDATSMessageDto _selectedMessage;
        public DMDATSMessageDto SelectedMessage
        {
            get => _selectedMessage;
            set => SetProperty(ref _selectedMessage, value);
        }

        private DMDATSIntervalDto _intervalConfig;
        public DMDATSIntervalDto IntervalConfig
        {
            get => _intervalConfig;
            set => SetProperty(ref _intervalConfig, value);
        }

        private List<DMDATSMessageDto> _originalMessages;
        private DMDATSIntervalDto _originalInterval;

        private ObservableCollection<DMDATSMessageDto> _dmdATSMessages = new();
        public ObservableCollection<DMDATSMessageDto> DMDATSMessages
        {
            get => _dmdATSMessages;
            set => SetProperty(ref _dmdATSMessages, value);
        }

        public ICollectionView DMDATSMessagesView { get; private set; }

        private eATSType _atsType = eATSType.單線雙向_一般車_中間站;
        public eATSType ATSType
        {
            get => _atsType;
            set => SetProperty(ref _atsType, value);
        }

        private readonly Dictionary<string, string[]> _replacePatterns = new()
        {
            // 進站倒數1：找「進站倒數超過X秒」或「進站倒數介於X秒」
            ["StationArrival1"] = new[]
            {
                @"(?<=進站倒數超過)\d+(?=秒)",
                @"(?<=進站倒數介於)\d+(?=秒)"
            },
            // 進站倒數2：找「～Y秒」或「進站倒數低於Y秒」
            ["StationArrival2"] = new[]
            {
                @"(?<=[~～])\d+(?=秒)",
                @"(?<=進站倒數低於)\d+(?=秒)"
            },
            // 間隔顯示1~3
            ["IntervalDisplay1"] = new[]
            {
                @"(?<=進站倒數超過\d+秒\s+間格顯示設定)\d+(?=秒)",
            },
            ["IntervalDisplay2"] = new[]
            {
                @"(?<=進站倒數介於\d+秒[~～]\d+秒\s+間格顯示設定)\d+(?=秒)"
            },
            ["IntervalDisplay3"] = new[]
            {
                @"(?<=進站倒數低於\d+秒\s+間格顯示設定)\d+(?=秒)"
            },
        };
        #endregion

        #region DelegateCommands

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??= new DelegateCommand(ExcuteConfirmCommand);
        private void ExcuteConfirmCommand()
        {
            var itemToUpdate = SelectedMessage;

            //資料庫

            UpdateSnapshot();
            IsEditMode = false;
        }

        private DelegateCommand _editCommand;
        public DelegateCommand EditCommand => _editCommand ??= new DelegateCommand(ExcuteEditCommand);
        private void ExcuteEditCommand()
        {
            IsEditMode = true;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(ExcuteCancelCommand);
        private void ExcuteCancelCommand()
        {
            if (!HasChanges())
            {
                IsEditMode = false;
                return;
            }

            DMDATSMessages = new ObservableCollection<DMDATSMessageDto>(
                _originalMessages.Select(m => new DMDATSMessageDto
                {
                    Id = m.Id,
                    ATSType = m.ATSType,
                    Header = m.Header,
                    CDU = m.CDU,
                    PDU = m.PDU
                })
            );

            IntervalConfig = new DMDATSIntervalDto
            {
                StationArrival1 = _originalInterval.StationArrival1,
                StationArrival2 = _originalInterval.StationArrival2,
                IntervalDisplay1 = _originalInterval.IntervalDisplay1,
                IntervalDisplay2 = _originalInterval.IntervalDisplay2,
                IntervalDisplay3 = _originalInterval.IntervalDisplay3
            };

            DMDATSMessagesView = CollectionViewSource.GetDefaultView(DMDATSMessages);
            DMDATSMessagesView.Filter = CollectionViewFilter;
            DMDATSMessagesView.Refresh();

            RaisePropertyChanged(nameof(DMDATSMessages));
            RaisePropertyChanged(nameof(IntervalConfig));
            RaisePropertyChanged(nameof(DMDATSMessagesView));

            IsEditMode = false;
        }

        private DelegateCommand<eATSType?> _selectionChangedCommand;
        public DelegateCommand<eATSType?> SelectionChangedCommand => _selectionChangedCommand ??= new DelegateCommand<eATSType?>(ExcuteSelectionChangedCommand);
        private void ExcuteSelectionChangedCommand(eATSType? filetr)
        {
            if (filetr is eATSType atsType)
            {
                ATSType = atsType;
                DMDATSMessagesView.Refresh();
            }
        }

        private DelegateCommand<SelectionChangedEventArgs> _intervalSelectionChangedCommand;
        public DelegateCommand<SelectionChangedEventArgs> IntervalSelectionChangedCommand => _intervalSelectionChangedCommand ??= new DelegateCommand<SelectionChangedEventArgs>(ExcuteIntervalSelectionChangedCommand);
        private void ExcuteIntervalSelectionChangedCommand(SelectionChangedEventArgs args)
        {
            // 拿 Tag
            var combo = args.OriginalSource as ComboBox;
            var tag = combo?.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            // 拿新的秒數
            var newValue = args.AddedItems.Count > 0
                ? (int)args.AddedItems[0]
                : 0;

            // UpdateHeadersByTag 只負責取代 Messages 裡的文字
            UpdateHeadersByTag(tag, newValue);
        }
        #endregion
        
        #region Private method
        private bool CollectionViewFilter(object item)
        {
            if (item is DMDATSMessageDto currentItem)
            {
                return currentItem.ATSType == ATSType;
            }
            return false;
        }
        private void UpdateHeadersByTag(string tag, int newVal)
        {
            if (!_replacePatterns.TryGetValue(tag, out var patterns))
                return;

            foreach (var msg in DMDATSMessages)
            {
                var header = msg.Header;
                foreach (var pat in patterns)
                {
                    var regex = new Regex(pat);
                    if (regex.IsMatch(header))
                    {
                        header = regex.Replace(header, newVal.ToString(), 1);
                        break;
                    }
                }
                msg.Header = header;
            }
            DMDATSMessagesView.Refresh();
        }
        private bool HasChanges()
        {
            foreach (var current in DMDATSMessages)
            {
                var original = _originalMessages.FirstOrDefault(m => m.Id == current.Id);
                if (original == null)
                    return true;

                if (original.ATSType != current.ATSType ||
                    original.Header != current.Header ||
                    original.CDU != current.CDU ||
                    original.PDU != current.PDU)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateSnapshot()
        {
            _originalMessages = DMDATSMessages
                .Select(m => new DMDATSMessageDto
                {
                    Id = m.Id,
                    ATSType = m.ATSType,
                    Header = m.Header,
                    CDU = m.CDU,
                    PDU = m.PDU
                }).ToList();

            _originalInterval = new DMDATSIntervalDto
            {
                StationArrival1 = IntervalConfig.StationArrival1,
                StationArrival2 = IntervalConfig.StationArrival2,
                IntervalDisplay1 = IntervalConfig.IntervalDisplay1,
                IntervalDisplay2 = IntervalConfig.IntervalDisplay2,
                IntervalDisplay3 = IntervalConfig.IntervalDisplay3
            };
        }
        private async Task InitDataAsync()
        {
            IntervalConfig = DesignTimeDatas.DMDATSInterval;
            DMDATSMessages = new ObservableCollection<DMDATSMessageDto>(DesignTimeDatas.DMDATSMessages);

            DMDATSMessagesView = CollectionViewSource.GetDefaultView(DMDATSMessages);
            DMDATSMessagesView.Filter = CollectionViewFilter;

            RaisePropertyChanged(nameof(DMDATSMessages));
            RaisePropertyChanged(nameof(IntervalConfig));
            RaisePropertyChanged(nameof(DMDATSMessagesView));

            UpdateSnapshot();
        }
        #endregion
    }
}
