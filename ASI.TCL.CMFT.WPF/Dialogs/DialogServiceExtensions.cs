using System;
using System.Linq;
using System.Windows;
using ASI.TCL.CMFT.WPF.Logger;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Dialogs
{
    public static class DialogServiceExtensions
    {
        public static void Close<TDialogWindow>(this IDialogService dialogService)
        {
            Application.Current?.Windows
                .OfType<IDialogWindow>()
                .SingleOrDefault(dialogWindow => dialogWindow.Content?.GetType() == typeof(TDialogWindow))?
                .Close();
        }
        public static bool IsOpened<TDialogWindow>(this IDialogService dialogService)
        {
            return Application.Current?.Windows
                .OfType<IDialogWindow>()
                .Any(dialogWindow => dialogWindow.Content?.GetType() == typeof(TDialogWindow)) ?? false;
        }
        public static void ShowDialog(this IDialogService dialogService, CustomDialogWindowSetting customDialogWindowSetting, string name, IDialogParameters parameters, Action<IDialogResult> callback) 
            
        {
            // 從容器解析視圖
            var container = ContainerLocator.Container;
            var view = container.Resolve<object>(name);

            if (view is not FrameworkElement frameworkElement)
            {
                throw new InvalidOperationException($"The resolved view '{name}' is not a FrameworkElement.");
            }

            // 自動綁定 ViewModel
            if (frameworkElement.DataContext == null)
            {
                ViewModelLocationProvider.AutoWireViewModelChanged(frameworkElement, (viewElement, vm) => { frameworkElement.DataContext = vm; });
            }

            if (frameworkElement.DataContext is not IDialogAware dialogAware) 
                return;
            
            // 創建自定義窗口
            var window = new CustomDialogWindow
            {
                Title = dialogAware.Title,
                Content = frameworkElement
            };
            customDialogWindowSetting.ApplyTo(window);
            dialogAware.RequestClose += result =>
            {
                if (window is IDialogWindow dialogWindow)
                {
                    dialogWindow.Result = result;
                    window.Close();
                }
            };
            window.Closed += (sender, e) =>
            {
                if (window is IDialogWindow dialogWindow)
                {
                    callback?.Invoke(dialogWindow.Result);
                }
            };
            dialogAware.OnDialogOpened(parameters); // 當窗口關閉時觸發回調
            // 顯示對話框
            window.ShowDialog();

        }
        public static void Show(this IDialogService dialogService, CustomDialogWindowSetting customDialogWindowSetting, string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            // 從容器解析視圖
            var container = ContainerLocator.Container;
            var view = container.Resolve<object>(name);

            if (view is not FrameworkElement frameworkElement)
            {
                throw new InvalidOperationException($"The resolved view '{name}' is not a FrameworkElement.");
            }

            // 自動綁定 ViewModel
            if (frameworkElement.DataContext == null)
            {
                ViewModelLocationProvider.AutoWireViewModelChanged(frameworkElement, (viewElement, vm) => { frameworkElement.DataContext = vm; });
            }

            if (frameworkElement.DataContext is not IDialogAware dialogAware)
                return;

            // 創建自定義窗口

            //使用customDialogWindowSetting的設定
            var window = new CustomDialogWindow()
            {
                Title = dialogAware.Title,
                Content = frameworkElement
            };
            customDialogWindowSetting.ApplyTo(window);
            dialogAware.RequestClose += result =>
            {
                if (window is IDialogWindow dialogWindow)
                {
                    dialogWindow.Result = result;
                    window.Close();
                }
            };
            window.Closed += (sender, e) =>
            {
                if (window is IDialogWindow dialogWindow)
                {
                    callback?.Invoke(dialogWindow.Result);
                }
            };
            dialogAware.OnDialogOpened(parameters); // 當窗口關閉時觸發回調
            // 顯示對話框
            window.Show();

        }
        public static bool IsDialogHostOpened(this IDialogService dialogService, string dialogIdentifier = null)
        {
            if (!(dialogService is ExtendedPrismDialogService extendedPrismDialogService))
                throw new NullReferenceException($"{nameof(IDialogService)} must be a {nameof(ExtendedPrismDialogService)}");

            return extendedPrismDialogService.IsDialogHostOpened(dialogIdentifier);
        }
        public static void ShowDialogHost(this IDialogService dialogService, string name, IDialogParameters parameters, Action<IDialogResult> callback, string dialogIdentifier = null)
        {
            if (!(dialogService is ExtendedPrismDialogService extendedPrismDialogService))
                throw new NullReferenceException($"{nameof(IDialogService)} must be a {nameof(ExtendedPrismDialogService)}");

            extendedPrismDialogService.ShowDialogHost(name, dialogIdentifier, parameters, callback);
        }
        public static void ShowMessageBox(this IDialogService dialogService, string message = "Message", string title = "Title", Action<IDialogResult> callback = null, eDialogFooterButtons dialogFooterButtons = eDialogFooterButtons.OK, PackIconKind messageBoxIcons = PackIconKind.None, string messageBoxColor = "#8e8e8e", string dialogIdentifier = null)
        {
            var dialogParameters = new DialogParameters()
            {
                  { "message", message }
                , { "title", title }
                , { "dialogFooterButtons", dialogFooterButtons }
                , { "messageBoxIcons", messageBoxIcons }
                , { "messageBoxColor", messageBoxColor }
            };
            dialogService.ShowDialogHost(nameof(MessageBoxView), dialogParameters, callback  , dialogIdentifier);
        }
        public static void Success(this IDialogService dialogService, string message = "Message", Action<IDialogResult> callback = null, string dialogIdentifier = null)
        {
            var log = ContainerLocator.Container.Resolve<ILogService>();
            log.Info($"顯示成功訊息：{message}");
            dialogService.ShowMessageBox(message, "成功", callback, eDialogFooterButtons.OK, PackIconKind.CheckCircle, "#38B035", dialogIdentifier);
        }

        public static void Information(this IDialogService dialogService, string message = "Message", Action<IDialogResult> callback = null, string dialogIdentifier = null)
        {
            var log = ContainerLocator.Container.Resolve<ILogService>();
            log.Info($"顯示資訊訊息：{message}");
            dialogService.ShowMessageBox(message, "資訊", callback, eDialogFooterButtons.OK, PackIconKind.InformationVariantCircle, "#88ceec", dialogIdentifier);
        }

        public static void Error(this IDialogService dialogService, string message = "Message", Action<IDialogResult> callback = null, string dialogIdentifier = null)
        {
            var log = ContainerLocator.Container.Resolve<ILogService>();
            log.Info($"顯示錯誤訊息：{message}");
            dialogService.ShowMessageBox(message, "錯誤", callback, eDialogFooterButtons.OK, PackIconKind.AlertCircle, "#C73E3A", dialogIdentifier);
        }

        public static void Warning(this IDialogService dialogService, string message = "Message", Action<IDialogResult> callback = null, string dialogIdentifier = null)
        {
            var log = ContainerLocator.Container.Resolve<ILogService>();
            log.Info($"顯示警告訊息：{message}");
            dialogService.ShowMessageBox(message, "警告", callback, eDialogFooterButtons.OK, PackIconKind.Alert, "#F05E1C", dialogIdentifier);
        }

        public static void IsDelete(this IDialogService dialogService, string message = "Message", Action<IDialogResult> callback = null, string dialogIdentifier = null)
        {
            var log = ContainerLocator.Container.Resolve<ILogService>();
            log.Info($"顯示刪除確認訊息：{message}");
            dialogService.ShowMessageBox(message, "刪除確認", callback, eDialogFooterButtons.OKCancel, PackIconKind.AlertCircle, "#F05E1C", dialogIdentifier);
        }
    }
}