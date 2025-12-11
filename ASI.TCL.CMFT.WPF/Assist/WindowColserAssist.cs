using System.Windows;
using System.Windows.Input;
using ASI.TCL.CMFT.WPF.Applications;

namespace ASI.TCL.CMFT.WPF.Assist
{
    //public class WindowColserAssist
//{
//    public static readonly DependencyProperty EnableWindowClosingProperty = DependencyProperty.RegisterAttached(
//        "EnableWindowClosing"
//        , typeof(bool)
//        , typeof(WindowColserAssist)
//        , new PropertyMetadata(false, OnEnableWindowClosingChanged));
//    private static void OnEnableWindowClosingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
//    {
//        //這邊把behind code寫好的東西移過來
//        if (dependencyObject is Window window)
//        {
//            window.Loaded += (s1, e) =>
//            {
//                if (window.DataContext is ICloseWindow viewModel)
//                {
//                    //將viewModel跟view關聯
//                    viewModel.Close += window.Close;

//                    window.Closing += (s2, arg) =>
//                    {
//                        arg.Cancel = !viewModel.CanClose();
//                    };
//                }
//            };
//        }
//    }
//    public static bool GetEnableWindowClosing(DependencyObject obj)
//    {
//        return (bool)obj.GetValue(EnableWindowClosingProperty);
//    }
//    public static void SetEnableWindowClosing(DependencyObject obj, bool value)
//    {
//        obj.SetValue(EnableWindowClosingProperty, value);
//    }

//    public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
//        "DialogResult"
//        , typeof(bool?)
//        , typeof(WindowColserAssist)
//        , new PropertyMetadata(DialogResultChanged));
//    private static void DialogResultChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
//    {
//        //這邊把behind code寫好的東西移過來
//        if (dependencyObject is Window window)
//        {
//            if (window.DataContext is ICloseWindow viewModel)
//            {
//                window.Closing += (s1, e) =>
//                {
//                    window.DialogResult = eventArgs.NewValue as bool?;
//                };
//            };
//        }
//    }
//    public static void SetDialogResult(Window target, bool? value)
//    {
//        target.SetValue(DialogResultProperty, value);
//    }
//}
    public class WindowColserAssist
    {
        public static readonly DependencyProperty EnableWindowClosingProperty = DependencyProperty.RegisterAttached(
            "EnableWindowClosing"
            , typeof(bool)
            , typeof(WindowColserAssist)
            , new PropertyMetadata(false, OnEnableWindowClosingChanged));
        private static void OnEnableWindowClosingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is Window window)
            {
                window.Loaded += (s1, e) =>
                {
                    if (window.DataContext is ICloseWindow viewModel)
                    {
                        //將viewModel跟view關聯
                        viewModel.Close += window.Close;

                        // Handle Closing event
                        window.Closing += (s2, arg) =>
                        {
                            // 檢查 ViewModel 的 CanClose 方法
                            if (!viewModel.CanClose())
                            {
                                arg.Cancel = true; // 阻止關閉
                            }
                            else
                            {
                                // 檢查命令是否可以執行
                                var closingCommand = GetClosingCommand(window);
                                if (closingCommand?.CanExecute(arg) == false)
                                {
                                    arg.Cancel = true; // 阻止關閉
                                }
                                else
                                {
                                    closingCommand?.Execute(arg); // 執行命令
                                }
                            }
                        };

                        // Handle Closed event
                        window.Closed += (s2, e2) =>
                        {
                            // 執行 ClosedCommand
                            var closedCommand = GetClosedCommand(window);
                            if (closedCommand?.CanExecute(null) == true)
                            {
                                closedCommand.Execute(null);
                            }
                        };
                    }
                };
            }
        }
        public static bool GetEnableWindowClosing(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(EnableWindowClosingProperty);
        }
        public static void SetEnableWindowClosing(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(EnableWindowClosingProperty, value);
        }

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
            "DialogResult"
            , typeof(bool?)
            , typeof(WindowColserAssist)
            , new PropertyMetadata(DialogResultChanged));
        private static void DialogResultChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            //這邊把behind code寫好的東西移過來
            if (dependencyObject is Window window)
            {
                if (window.DataContext is ICloseWindow)
                {
                    window.Closing += (s1, e) =>
                    {
                        window.DialogResult = eventArgs.NewValue as bool?;
                    };
                }
            }
        }
        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        public static readonly DependencyProperty ClosingCommandProperty = DependencyProperty.RegisterAttached(
            "ClosingCommand",
            typeof(ICommand),
            typeof(WindowColserAssist),
            new PropertyMetadata(null));
        public static ICommand GetClosingCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosingCommandProperty);
        }
        public static void SetClosingCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosingCommandProperty, value);
        }

        public static readonly DependencyProperty ClosedCommandProperty = DependencyProperty.RegisterAttached(
            "ClosedCommand",
            typeof(ICommand),
            typeof(WindowColserAssist),
            new PropertyMetadata(null));
        public static ICommand GetClosedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosedCommandProperty);
        }
        public static void SetClosedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosedCommandProperty, value);
        }
    }
}