using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class FrameworkAssist
    {
        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached("LoadedCommand", typeof(ICommand), typeof(FrameworkAssist), new PropertyMetadata(OnLoadedCommandChanged));
        public static ICommand GetLoadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LoadedCommandProperty);
        }
        public static void SetLoadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LoadedCommandProperty, value);
        }
        private static void OnLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;

            if (e.OldValue != null)
                control.Loaded -= OnLoaded;
            if (e.NewValue != null)
                control.Loaded += OnLoaded;
        }
        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 跳過設計模式下的執行
            if (DesignerProperties.GetIsInDesignMode((DependencyObject)sender))
                return;

            if (sender is DependencyObject element)
            {
                var command = GetLoadedCommand(element);
                var commandParameter = GetLoadedCommandParameter(element);
                if (command != null && command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        public static readonly DependencyProperty LoadedCommandParameterProperty =
            DependencyProperty.RegisterAttached("LoadedCommandParameter", typeof(object), typeof(FrameworkAssist), new PropertyMetadata(null));
        public static object GetLoadedCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(LoadedCommandParameterProperty);
        }
        public static void SetLoadedCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(LoadedCommandParameterProperty, value);
        }

        public static readonly DependencyProperty UnloadedCommandProperty =
            DependencyProperty.RegisterAttached("UnloadedCommand", typeof(ICommand), typeof(FrameworkAssist), new PropertyMetadata(OnUnloadedCommandChanged));
        public static ICommand GetUnloadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(UnloadedCommandProperty);
        }
        public static void SetUnloadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(UnloadedCommandProperty, value);
        }
        private static void OnUnloadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control)) return;
   
            if (e.OldValue != null)
                control.Unloaded -= OnUnloaded;
            if (e.NewValue != null)
                control.Unloaded += OnUnloaded;
        }
        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // 跳過設計模式下的執行
            if (DesignerProperties.GetIsInDesignMode((DependencyObject)sender))
                return;

            if ( sender is DependencyObject element )
            {
                var command = GetUnloadedCommand(element);
                var commandParameter = GetUnloadedCommandParameter(element);
                if (command != null && command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        public static readonly DependencyProperty UnloadedCommandParameterProperty =
            DependencyProperty.RegisterAttached("UnloadedCommandParameter", typeof(object), typeof(FrameworkAssist), new PropertyMetadata(null));
        public static object GetUnloadedCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(UnloadedCommandParameterProperty);
        }
        public static void SetUnloadedCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(UnloadedCommandParameterProperty, value);
        }
    }
}