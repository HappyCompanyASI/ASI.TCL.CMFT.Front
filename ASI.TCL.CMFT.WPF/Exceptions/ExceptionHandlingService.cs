using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ASI.TCL.CMFT.WPF.Exceptions
{
    public class ExceptionHandlingService : IExceptionHandlingService
    {
        public void Initialize(Application app)
        {
            // UI 線程未捕獲異常處理事件（UI 主線程）
            app.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            // Task 線程內未捕獲異常處理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            // 非 UI 線程未捕獲異常處理事件(例如自己創建的一個子線程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ShowExceptionWindow("UI Thread", e.Exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ShowExceptionWindow("Task Thread", e.Exception.InnerException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowExceptionWindow("Non-UI Thread", (Exception)e.ExceptionObject);
        }

        private static DispatcherOperationStatus dispatcherOperationStatus = DispatcherOperationStatus.Completed;
        private static void ShowExceptionWindow(string from, Exception e)
        {
            void OpenExceptionWindowHandle()
            {

                var dispatcherOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action<string, Exception>(OpenExceptionWindow), from, e);
                dispatcherOperationStatus = dispatcherOperation.Status;
                while (true)
                {
                    if (dispatcherOperationStatus != DispatcherOperationStatus.Completed)
                    {
                        //Wait執行後彈出Exception視窗,並且於視窗關閉後回傳DispatcherOperationStatus.Completed,才會繼續往下執行
                        dispatcherOperationStatus = dispatcherOperation.Wait(TimeSpan.FromMilliseconds(1000));

                    }
                    else
                    {
                        //ExceptionWindow關閉後
                        break;
                    }
                }

                // 確保在 UI 執行緒中檢查是否有開啟的視窗
                var hasOpenWindows = false;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    hasOpenWindows = Application.Current.Windows.Count > 0;
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.IsVisible)
                        {
                            hasOpenWindows = true;
                            break;
                        }
                    }
                });

                if (hasOpenWindows == false)
                {
                    var processName = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
                    var workers = Process.GetProcessesByName(processName);

                    foreach (var worker in workers)
                    {
                        worker.Kill();
                        worker.WaitForExit();
                        worker.Dispose();
                    }
                    return;
                }

                Dispatcher.Run();
            }

            var openExceptionWindowThread = new Thread(OpenExceptionWindowHandle);
            while (true)
            {
                Thread.Sleep(1000);
                if (dispatcherOperationStatus == DispatcherOperationStatus.Completed)
                {
                    openExceptionWindowThread.SetApartmentState(ApartmentState.STA);
                    openExceptionWindowThread.Start();
                    break;
                }
            }
        }
        private static void OpenExceptionWindow(string from, Exception e)
        {
            var view = new ExceptionWindow();
            var viewModel = new ExceptionWindowViewModel();
            viewModel.Title = from;
            viewModel.vmpDefaultMessage = e.Message;
            viewModel.vmpCustomMessage = e.InnerException?.Message;
            viewModel.vmpStackInfo = e.StackTrace;

            view.DataContext = viewModel;
            var result = view.ShowDialog();

            if (result == true)
            {
                //寫LOG
            }
        }
    }
}