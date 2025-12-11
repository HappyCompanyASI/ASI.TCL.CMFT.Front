using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Module.SYS.Datas;
using ASI.TCL.CMFT.WPF.Module.SYS.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSInappropriateWordSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;
        

        #region Constructors
        public SYSInappropriateWordSettingsViewModel()
        {
        }
        public SYSInappropriateWordSettingsViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            
            _dialogService = dialogService;
            
            _eventAggregator = eventAggregator;
            

            InitDataAsync().Await(null, (ex) => throw ex);
        }
        #endregion

        #region Properties
        public bool IsItemOperation => SelectedWord != null;

        private string _selectedWord;
        public string SelectedWord
        {
            get => _selectedWord;
            set
            {
                SetProperty(ref _selectedWord, value);
                RaisePropertyChanged(nameof(IsItemOperation));
            }
        }

        private ObservableCollection<string> _sysInappropriateWords = new();
        public ObservableCollection<string> SYSInappropriateWords
        {
            get => _sysInappropriateWords;
            set => SetProperty(ref _sysInappropriateWords, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand => _addItemCommand ??= new DelegateCommand(ExcuteAddItemCommand);
        private  void ExcuteAddItemCommand()
        {
            _dialogService.ShowDialogHost(nameof(SYSInappropriateWordSettingsDialogView), null, result =>
            {
                if (result.Result == ButtonResult.OK && result.Parameters.ContainsKey("InappropriateWord"))
                {
                    string inappropriateWord = result.Parameters.GetValue<string>("InappropriateWord");

                    SYSInappropriateWords.Add(inappropriateWord);
                    SelectedWord = inappropriateWord; // 選取新增的項目
                }
            }, "MainDialogHost");
        }

        private DelegateCommand _deleteItemCommand;
        public DelegateCommand DeleteItemCommand => _deleteItemCommand ??= new DelegateCommand(ExcuteDeleteItemCommand);
        private void ExcuteDeleteItemCommand()
        {

            var itemToDelete = SelectedWord;

            if (itemToDelete == null)
                return;

            _dialogService.IsDelete($"確定要刪除\"{itemToDelete}\"這筆訊息嗎？", async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    SelectedWord = null; // 清除選取

                    SYSInappropriateWords.Remove(itemToDelete);
                }
            }, "MainDialogHost");
        }
        #endregion

        #region Private Methods
        private async Task InitDataAsync()
        {
            // 更新 ViewModel 屬性
            SYSInappropriateWords = new ObservableCollection<string>(DesignTimeDatas.DMDBlockList);
        }
        #endregion
    }
}
