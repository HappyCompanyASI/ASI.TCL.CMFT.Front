using System;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.IO;
using System.Reflection;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ASI.TCL.CMFT.WPF.Applications;
using ASI.TCL.CMFT.WPF.CMFTApp.Datas;
using ASI.TCL.CMFT.WPF.CMFTApp.Views;
using ASI.TCL.CMFT.WPF.Events;
using ASI.TCL.CMFT.WPF.Module.Auth.Events;
using ASI.TCL.CMFT.WPF.Utilities;
using ASI.TCL.CMFT.WPF.Web;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using static ASI.TCL.CMFT.Messages.Auth.Commands;

namespace ASI.TCL.CMFT.WPF.CMFTApp
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private readonly IApiClient _apiClient;

        private TaskCompletionSource<bool> _loginCompletionSource;

        #region Constructors
        public MainWindowViewModel()
        {
            if (WPFHelper.IsDesignTime)
            {
                ErrorMessage = "無法與Server連線";
            }
        }
        public MainWindowViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IApiClient apiClient)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _apiClient = apiClient;


            //登入畫面
            IsAuthenticated = false;
            Account = string.Empty;
            Password = string.Empty;
            LoginStateMessage = string.Empty;

            //主畫面
            ErrorMessage = "";
            WPFApplicationCommands.DefaultViewName = nameof(MainView);
            WPFApplicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);

            //_eventBus.Subscribe(ReceiveMessage);

            _eventAggregator.GetEvent<LogoutEvent>()
                            .Subscribe(async () => await Logout(), ThreadOption.UIThread);
            _eventAggregator.GetEvent<RestartEvent>()
                            .Subscribe(async () => await Restart(), ThreadOption.UIThread);
        }
        #endregion

        #region Properties
        // 登入畫面
        private bool _hasExecutedLoaded = false; // 開發時使用，確保 LoadedCommand 只執行一次的flag

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => SetProperty(ref _isAuthenticated, value);
        }
        private string _account;
        public string Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private string _loginStateMessage;
        public string LoginStateMessage
        {
            get => _loginStateMessage;
            set => SetProperty(ref _loginStateMessage, value);
        }

        // 主畫面
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        // 通訊設備即時告警
        private int _unConfirmedEquipAlarmCount;
        public int UnConfirmedEquipAlarmCount
        {
            get => _unConfirmedEquipAlarmCount;
            set => SetProperty(ref _unConfirmedEquipAlarmCount, value);
        }
        #endregion

        #region DelegateCommands
        // 登入畫面
        private DelegateCommand _settingCommand;
        public DelegateCommand SettingCommand => _settingCommand ??= new DelegateCommand(ExcuteSettingCommand);
        private void ExcuteSettingCommand()
        {
            //_dialogService.ShowDialogHost(nameof(LoginSettingDialog), null, _ => { }, "LoginDialogHost");
        }

        private DelegateCommand _loadedCommand;
        public DelegateCommand LoadedCommand => _loadedCommand ??= new DelegateCommand(ExcuteLoadedCommand);
        private async void ExcuteLoadedCommand()
        {
            //這一區是開發時方便登入暫時這樣使用
            //materialDesign畫面渲染時會導致Loaded事件被多次觸發，這裡只執行一次登入邏輯
            if (_hasExecutedLoaded)
                return;
            _hasExecutedLoaded = true;

            await Login();
        }

        private DelegateCommand _loginCommand;
        public DelegateCommand LoginCommand => _loginCommand ??= new DelegateCommand(ExcuteLoginCommand);
        private async void ExcuteLoginCommand() => await Login();

        private DelegateCommand<KeyEventArgs> _keyDownCommand;
        public DelegateCommand<KeyEventArgs> KeyDownCommand => _keyDownCommand ??= new DelegateCommand<KeyEventArgs>(ExcuteKeyDownCommand);
        private async void ExcuteKeyDownCommand(KeyEventArgs args)
        {
            if (args.Key == Key.Return) await Login();
        }

        private DelegateCommand _leaveCommand;
        public DelegateCommand LeaveCommand => _leaveCommand ??= new DelegateCommand(ExcuteLeaveCommand);
        private async void ExcuteLeaveCommand()
        {
            await Leave();
        }

        // 主畫面
        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(ExecuteNavigateCommand);
        private void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException(nameof(navigationPath), @"導航失敗");
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, navigationPath);
        }
        #endregion

        #region PrivateMethods

        private async Task Login()
        {
            LoginStateMessage = "登錄中請稍後....";

            //// 建立等待 EventBus 回應的 TaskCompletionSource
            //_loginCompletionSource = new TaskCompletionSource<bool>();
            //try
            //{
            //    // 初始化資料庫 
            //    await _dbInitializer.InitializeAsync();
            //    // TODO 初始化資料庫結果處理

            //    // 執行登入命令(暫定以superadmin登入)
            //    await _requestHandler.HandleCommand(new Commands.LoginCommand
            //    {
            //        Account = "superadmin",
            //        Password = "superadmin"
            //    }, _authApplicationService.Handle);

            //    // 等待 EventBus 回應，最多 10 秒
            //    var timeoutTask = Task.Delay(10000);
            //    var completedTask = await Task.WhenAny(_loginCompletionSource.Task, timeoutTask);

            //    if (completedTask == timeoutTask)
            //    {
            //        // 10 秒逾時，沒收到 EventBus 回應
            //        LoginStateMessage = "登入回應逾時，請重新嘗試登入。";
            //    }
            //    // 如果收到 EventBus 回應，由 ReceiveMessage 處理 UI 更新
            //}
            //catch (Exception ex)
            //{
            //    LoginStateMessage = $"登入發生錯誤：{ex.Message}";
            //}

            var token = await _apiClient.PostAsync<LoginCommand, string>("api/auth/login", new LoginCommand
            {
                Account = "admin",
                Password = "admin"
            });
            if (!string.IsNullOrEmpty(token))
            {
                var userName = "admin";
                var userRole = "admin";

                //登入成功
                IsAuthenticated = true;
                LoginStateMessage = $"歡迎 {userName} 登入, 角色: {userRole}";

                //更新畫面狀態
                RaisePropertyChanged(nameof(IsAuthenticated));
                RaisePropertyChanged(nameof(LoginStateMessage));

                var info = new LoginInfo
                {
                    UserName = "UserName",
                    UserRole = "UserRole"
                };

                _eventAggregator.GetEvent<LoginEvent>().Publish(info);
            }
            else
            {
                //登入失敗
                IsAuthenticated = false;
                LoginStateMessage = "登入失敗，請檢查帳號或密碼是否正確。";
                RaisePropertyChanged(nameof(IsAuthenticated));
                RaisePropertyChanged(nameof(LoginStateMessage));
            }
        }

        private async Task Leave()
        {
            LoginStateMessage = "應用程式關閉中,請稍後...";
            RaisePropertyChanged(nameof(LoginStateMessage));
            await Task.Delay(2000);

            //離開結果通知或錯誤處理

            //關閉應用程式(Application命名衝突需要明確表示)
            System.Windows.Application.Current.Shutdown();
        }
        private async Task Logout()
        {
            //執行登出命令
            //await _requestHandler.HandleCommand(new Commands.LogoutCommand(), _authApplicationService.Handle);

            //登出結果通知或錯誤處理

            Account = string.Empty;
            Password = string.Empty;
            LoginStateMessage = string.Empty;
            RaisePropertyChanged(nameof(Account));
            RaisePropertyChanged(nameof(Password));
            RaisePropertyChanged(nameof(LoginStateMessage));

            IsAuthenticated = false;
            RaisePropertyChanged(nameof(IsAuthenticated));
        }
        private async Task Restart()
        {
            try
            {
                // 1. 先執行後端登出邏輯，確保 Session/Token 清乾淨
                //await _requestHandler.HandleCommand(
                //    new Commands.LogoutCommand(),
                //    _authApplicationService.Handle);

                // 2. 取得「正確」的 exe 路徑
                //    如果是單檔發佈(single-file)，Location 可能是臨時提取的路徑。
                //    我們改用 AppContext.BaseDirectory + Assembly.GetEntryAssembly().GetName().Name + ".exe"
                string exeName = $"{Assembly.GetEntryAssembly().GetName().Name}.exe";
                string baseDir = AppContext.BaseDirectory; // 這個在單檔發佈也會指向提取後的目錄
                string exePathCandidate = Path.Combine(baseDir, exeName);

                string exePath;
                if (File.Exists(exePathCandidate))
                {
                    // 以該路徑為主
                    exePath = exePathCandidate;
                }
                else
                {
                    // 如果找不到，就退回用 Location（適用於 Debug 或 Framework 發佈）
                    exePath = Assembly.GetEntryAssembly().Location;
                }

                // 3. 確保新執行個體以相同工作目錄啟動
                var psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    WorkingDirectory = baseDir,
                    UseShellExecute = true
                };

                // 4. 為了避免「太快 Shutdown，造成 Logout 尚未跑完就關掉程式」，
                //    可以稍微延後 200ms，或至少讓 UI 有機會做最後更新
                await Task.Delay(2000);

                // 5. 啟動新執行個體
                Process.Start(psi);

                // 6. 關閉舊執行個體
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                // 如果啟動失敗，可以先顯示錯誤訊息
                MessageBox.Show($"重新啟動失敗：{ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Event Handlers
        //private void ReceiveMessage(IApplicationEvent evt)
        //{
        //    switch (evt)
        //    {
        //        case Notifications.LoginResultApplication n:

        //            // 通知等待中的 Login 方法
        //            _loginCompletionSource?.SetResult(n.Success);
                    
        //            //登入結果通知或錯誤處理

        //            if (n.Success)
        //            {
        //                //登入成功
        //                IsAuthenticated = true;
        //                LoginStateMessage = $"歡迎 {n.UserName} 登入, 角色: {n.UserRole}";

        //                //更新畫面狀態
        //                RaisePropertyChanged(nameof(IsAuthenticated));
        //                RaisePropertyChanged(nameof(LoginStateMessage));

        //                var info = new LoginInfo
        //                {
        //                    UserName = n.UserName,
        //                    UserRole = n.UserRole
        //                };

        //                _eventAggregator.GetEvent<LoginEvent>().Publish(info);
        //            }
        //            else
        //            {
        //                //登入失敗
        //                IsAuthenticated = false;
        //                LoginStateMessage = "登入失敗，請檢查帳號或密碼是否正確。";
        //                RaisePropertyChanged(nameof(IsAuthenticated));
        //                RaisePropertyChanged(nameof(LoginStateMessage));
        //            }

        //            break;

        //        case Notifications.LogoutResultApplication n:

        //            //登出結果通知或錯誤處理

        //            break;

        //        default:
        //            Debug.WriteLine($"❓ 未處理的事件: {evt.GetType().Name}");
        //            break;
        //    }
        //}
        #endregion

        #region Log
        public ObservableCollection<string> LogMessages { get; } = new();
        private void Log(string message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LogMessages.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            });
        }
        private DelegateCommand _clearLogCommand;
        public DelegateCommand ClearLogCommand => _clearLogCommand ??= new DelegateCommand(ExecuteClearLogCommand);
        private void ExecuteClearLogCommand() => LogMessages.Clear();
        #endregion

        #region 開發測試指令

        private DelegateCommand _simulatePICallCommand;
        public DelegateCommand SimulatePICallCommand => _simulatePICallCommand ??= new DelegateCommand(ExecuteSimulatePICallCommand);
        private void ExecuteSimulatePICallCommand()
        {
            // 模擬 PI 來電通知資料
            var piCallArgs = new PICallNotificationEventArgs(
                trainNumber: "C001",
                car: "1車",
                piNumber: "001",
                baseStation: "BTS-001"
            );

            // 發送 PI 來電通知事件
            _eventAggregator.GetEvent<PICallNotificationEvent>().Publish(piCallArgs);

            // 記錄到 Log
            Log($"模擬發送 PI 來電通知: {piCallArgs.TrainNumber}-{piCallArgs.Car} PI#{piCallArgs.PINumber} @ {piCallArgs.BaseStation}");
        }

        private DelegateCommand _simulateSICallCommand;
        public DelegateCommand SimulateSICallCommand => _simulateSICallCommand ??= new DelegateCommand(ExecuteSimulateSICallCommand);
        private void ExecuteSimulateSICallCommand()
        {
            // 模擬 SI 來電通知資料
            var siCallArgs = new SICallNotificationEventArgs(
                trainNumber: "C002",
                car: "2車",
                baseStation: "BTS-002"
            );

            // 發送 SI 來電通知事件
            _eventAggregator.GetEvent<SICallNotificationEvent>().Publish(siCallArgs);

            // 記錄到 Log
            Log($"模擬發送 SI 來電通知: {siCallArgs.TrainNumber}-{siCallArgs.Car} @ {siCallArgs.BaseStation}");
        }

        private DelegateCommand _simulateTetraCallCommand;
        public DelegateCommand SimulateTetraCallCommand => _simulateTetraCallCommand ??= new DelegateCommand(ExecuteSimulateTetraCallCommand);
        private void ExecuteSimulateTetraCallCommand()
        {
            // 模擬無線電來電通知資料
            var tetraCallArgs = new TetraCallNotificationEventArgs(
                phoneNumber: "50965",
                baseStation: "BTS-003",
                callType: "緊急"
            );

            // 發送無線電來電通知事件
            _eventAggregator.GetEvent<TetraCallNotificationEvent>().Publish(tetraCallArgs);

            // 記錄到 Log
            Log($"模擬發送無線電來電通知: {tetraCallArgs.PhoneNumber} @ {tetraCallArgs.BaseStation} ({tetraCallArgs.CallType})");
        }
        #endregion
    }
}
