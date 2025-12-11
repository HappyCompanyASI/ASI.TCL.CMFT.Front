using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Dialogs
{
    internal class ExtendedPrismDialogService : DialogService
    {
        private readonly IContainerExtension _containerExtension;
        public ExtendedPrismDialogService(IContainerExtension containerExtension ) : base(containerExtension)
        {
            _containerExtension = containerExtension;
        }
        public bool IsDialogHostOpened(string dialogIdentifier)
        {
            return DialogHost.IsDialogOpen(dialogIdentifier);
        }
        public void ShowDialogHost(string name, string dialogIdentifier, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            //利用view的名稱mapping生成view,並回傳實體,帶有viewModel
            var content = _containerExtension.Resolve<object>(name);

            //確認這個view是否是可Binding的控件(要有繼承自FrameworkElement)
            if (!(content is FrameworkElement dialogContent))
            {
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");
            }
            //再次確認viewModel是否有存在並設定
            AutowireViewModel(dialogContent);

            //取得viewModel,並確定是否有繼承Prism的IDialogAware
            if (!(dialogContent.DataContext is IDialogAware dialogAware))
            {
                throw new ArgumentException("A dialog's ViewModel must implement IDialogAware interface");
            }

            //將prism的開關事件對應至MaterialDesign
            var openedEventHandler = new DialogOpenedEventHandler((sender, args) =>
            {
                if (parameters is null)
                    parameters = new DialogParameters();
                dialogAware.OnDialogOpened(parameters);
            });
            var closedEventHandler = new DialogClosedEventHandler((sender, args) =>
            {
                
                dialogAware.OnDialogClosed();
            });

            dialogAware.RequestClose += res =>
            {
                if (DialogHost.IsDialogOpen(dialogIdentifier))
                {
                    callback(res);
                    DialogHost.Close(dialogIdentifier);
                }
            };


            var dispatcherFrame = new DispatcherFrame();
            _ = DialogHost.Show(dialogContent, dialogIdentifier, openedEventHandler, null, closedEventHandler)
                .ContinueWith(_ => dispatcherFrame.Continue = false);

            try
            {
                // tell users we're going modal
                ComponentDispatcher.PushModal();

                Dispatcher.PushFrame(dispatcherFrame);
            }
            finally
            {
                // tell users we're going non-modal
                ComponentDispatcher.PopModal();
            }
            dialogAware.RequestClose -= callback;
        }
        private static void AutowireViewModel(object viewOrViewModel)
        {
            if (viewOrViewModel is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }
        }
    }
}