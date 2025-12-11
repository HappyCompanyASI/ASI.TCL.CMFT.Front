using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public class SliderAssist
    {
        #region DragCompletedCommand
        public static readonly DependencyProperty DragStartedCommandProperty = DependencyProperty.RegisterAttached(
            "DragStartedCommand"
            , typeof(ICommand)
            , typeof(SliderAssist)
            , new PropertyMetadata(default(ICommand), OnDragStartedCommandChanged));
        public static ICommand GetDragStartedCommand(UIElement element) => (ICommand)element.GetValue(DragStartedCommandProperty);
        public static void SetDragStartedCommand(UIElement element, ICommand value) => element.SetValue(DragStartedCommandProperty, value);
        private static void OnDragStartedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is ICommand)
            {
                slider.Loaded += SliderOnLoaded_Started;
            }
        }
        private static void SliderOnLoaded_Started(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                slider.Loaded -= SliderOnLoaded_Started;
               
                if (slider.Template.FindName("PART_Track", slider) is Track track)
                {
                    track.Thumb.DragStarted += (dragCompletedSender, dragCompletedArgs) =>
                    {
                        ICommand command = GetDragStartedCommand(slider);
                        command.Execute(null);
                    };
                }
            }
        }
        #endregion

        #region DragDeltaCommand
        public static readonly DependencyProperty DragDeltaCommandProperty = DependencyProperty.RegisterAttached(
            "DragDeltaCommand"
            , typeof(ICommand)
            , typeof(SliderAssist)
            , new PropertyMetadata(default(ICommand), OnDragDeltaCommandChanged));
        public static ICommand GetDragDeltaCommand(UIElement element) => (ICommand)element.GetValue(DragDeltaCommandProperty);
        public static void SetDragDeltaCommand(UIElement element, ICommand value) => element.SetValue(DragDeltaCommandProperty, value);
        private static void OnDragDeltaCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is ICommand)
            {
                slider.Loaded += SliderOnLoaded_DragDelta;
            }
        }
        private static void SliderOnLoaded_DragDelta(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                slider.Loaded -= SliderOnLoaded_DragDelta;
                if (slider.Template.FindName("PART_Track", slider) is Track track)
                {
                    track.Thumb.DragDelta += (dragCompletedSender, dragCompletedArgs) =>
                    {
                        ICommand command = GetDragDeltaCommand(slider);
                        command.Execute(null);
                    };
                }
            }
        }
        #endregion

        #region DragCompletedCommand
        public static readonly DependencyProperty DragCompletedCommandProperty = DependencyProperty.RegisterAttached(
            "DragCompletedCommand"
            , typeof(ICommand)
            , typeof(SliderAssist)
            , new PropertyMetadata(default(ICommand), OnDragCompletedCommandChanged));
        public static ICommand GetDragCompletedCommand(DependencyObject d) => (ICommand)d.GetValue(DragCompletedCommandProperty);
        public static void SetDragCompletedCommand(DependencyObject d, ICommand value) => d.SetValue(DragCompletedCommandProperty, value);
        private static void OnDragCompletedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is ICommand)
            {
                slider.Loaded += SliderOnLoaded_DragCompleted;
            }
        }
        private static void SliderOnLoaded_DragCompleted(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                slider.Loaded -= SliderOnLoaded_DragCompleted;
                if (slider.Template.FindName("PART_Track", slider) is Track track)
                {
                    track.Thumb.DragCompleted += (dragCompletedSender, dragCompletedArgs) =>
                    {
                        ICommand command = GetDragCompletedCommand(slider);
                        command.Execute(null);
                    };
                }
            }
        }
        #endregion

        #region MoveToPointOnDrag
        public static readonly DependencyProperty MoveToPointOnDragProperty = DependencyProperty.RegisterAttached(
            "MoveToPointOnDrag"
            , typeof(bool)
            , typeof(SliderAssist)
            , new PropertyMetadata(OnMoveToPointOnDragChanged));
        public static bool GetMoveToPointOnDrag(DependencyObject d) => (bool)d.GetValue(MoveToPointOnDragProperty);
        public static void SetMoveToPointOnDrag(DependencyObject d, bool value) => d.SetValue(MoveToPointOnDragProperty, value);
        private static void OnMoveToPointOnDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is bool)
            {
                slider.MouseMove += SliderOnLoaded_MouseMove;
            }
        }
        private static void SliderOnLoaded_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Slider slider && e.LeftButton == MouseButtonState.Pressed)
            {
                var mouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                mouseButtonEventArgs.RoutedEvent = UIElement.PreviewMouseDownEvent;
                mouseButtonEventArgs.Source = e.Source;
                slider.RaiseEvent(mouseButtonEventArgs);
            }
        }
        #endregion
    }
}