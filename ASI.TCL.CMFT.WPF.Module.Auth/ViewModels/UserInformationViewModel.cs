using System;
using System.Windows.Threading;
using ASI.TCL.CMFT.WPF.Module.Auth.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Module.Auth.ViewModels
{
    public class UserInformationViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        
        private readonly IDialogService _dialogService;
        
        private readonly IEventAggregator _eventAggregator;
        private readonly DispatcherTimer _clockTimer;

        #region Constructors
        public UserInformationViewModel()
        {
            // 設計時預設值
            CurrentDateTime = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }
        public UserInformationViewModel( IDialogService dialogService, IEventAggregator eventAggregator)
        {
            
            _dialogService = dialogService;
            
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<LoginEvent>()
                           .Subscribe(UpdateUserInfo, ThreadOption.UIThread);

            // 初始化時間顯示
            InitializeClock();
        }
        #endregion

        #region Properties
        private string _userName = "Admin";
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        private string _userRole = "Admin";
        public string UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }

        private string _currentDateTime;
        public string CurrentDateTime
        {
            get => _currentDateTime;
            set => SetProperty(ref _currentDateTime, value);
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand _logoutCommand;
        public DelegateCommand LogoutCommand => _logoutCommand ??= new DelegateCommand(ExcuteLogoutCommand);
        private void ExcuteLogoutCommand()
        {
            _eventAggregator.GetEvent<LogoutEvent>().Publish();
        }

        private DelegateCommand _restartCommand;
        public DelegateCommand RestartCommand => _restartCommand ??= new DelegateCommand(ExecuteRestartCommand);
        private void ExecuteRestartCommand()
        {
            _eventAggregator.GetEvent<RestartEvent>().Publish();
        }
        #endregion

        #region PrivateMethods
        private void InitializeClock()
        {
            // 初始化現在時間
            CurrentDateTime = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");

            // 創建定時器，每秒更新一次
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                CurrentDateTime = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            };
            timer.Start();
        }

        private void UpdateUserInfo(LoginInfo info)
        {
            // info.UserName 帶入登入者名稱
            // info.UserRole 帶入登入者角色
            var userName = info.UserName;
            var role = info.UserRole;

            if (!string.IsNullOrEmpty(userName))
            {
                UserName = userName;
            }
            if (!string.IsNullOrEmpty(role))
            {
                UserRole = role;
            }

        }
        #endregion
    }
}
