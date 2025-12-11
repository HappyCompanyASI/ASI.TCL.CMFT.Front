using System.Windows;
using System.Windows.Controls;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class MarginAssist
    {
        #region Margin
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
            "Margin"
            , typeof(Thickness)
            , typeof(MarginAssist)
            , new UIPropertyMetadata(new Thickness(), OnMarginChanged));

        public static Thickness GetMargin(DependencyObject dependencyObject)
        {
            return (Thickness)dependencyObject.GetValue(MarginProperty);
        }
        public static void SetMargin(DependencyObject dependencyObject, Thickness value)
        {
            dependencyObject.SetValue(MarginProperty, value);
        }
        #endregion

        #region LastItemMargin
        public static readonly DependencyProperty LastItemMarginProperty = DependencyProperty.RegisterAttached(
            "LastItemMargin"
            , typeof(Thickness)
            , typeof(MarginAssist)
            , new UIPropertyMetadata(new Thickness(), OnMarginChanged));

        private static Thickness GetLastItemMargin(Panel panel)
        {
            return (Thickness)panel.GetValue(LastItemMarginProperty);
        }
        public static void SetLastItemMargin(DependencyObject dependencyObject, Thickness value)
        {
            dependencyObject.SetValue(LastItemMarginProperty, value);
        }
        #endregion

        private static void OnMarginChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            // Make sure this is put on a panel
            var panel = sender as Panel;
            if (panel == null) return;

            // Avoid duplicate registrations
            panel.Loaded -= OnPanelLoaded;
            panel.Loaded += OnPanelLoaded;

            if (panel.IsLoaded)
            {
                OnPanelLoaded(panel, null);
            }
        }
        private static void OnPanelLoaded(object sender, RoutedEventArgs eventArgs)
        {
            var panel = (Panel)sender;

            // Go over the children and set margin for them:
            for (var i = 0; i < panel.Children.Count; i++)
            {
                UIElement child = panel.Children[i];
                var fe = child as FrameworkElement;
                if (fe == null) continue;

                bool isLastItem = i == panel.Children.Count - 1;
                fe.Margin = isLastItem ? GetLastItemMargin(panel) : GetMargin(panel);
            }
        }
    }
}