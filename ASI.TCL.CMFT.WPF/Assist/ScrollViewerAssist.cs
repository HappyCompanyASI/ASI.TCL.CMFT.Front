using System.Windows;
using System.Windows.Controls;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class ScrollViewerAssist
    {
        #region AutoScrollToTop
        public static readonly DependencyProperty AutoScrollToTopProperty = DependencyProperty.RegisterAttached(
            "AutoScrollToTop"
            , typeof(bool)
            , typeof(ScrollViewerAssist)
            , new PropertyMetadata(false, OnAutoScrollToTopChanged));
        public static bool GetAutoScrollToTop(DependencyObject d) => (bool)d.GetValue(AutoScrollToTopProperty);
        public static void SetAutoScrollToTop(DependencyObject d, bool value) => d.SetValue(AutoScrollToTopProperty, value);
        private static void OnAutoScrollToTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (d is ScrollViewer scrollViewer && eventArgs.NewValue is bool)
            {
                scrollViewer.ScrollToTop();
                SetAutoScrollToTop(d, false);
            }
        }
        #endregion
    }
}