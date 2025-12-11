using System.Windows;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class SpacingAssist
    {
        #region Horizontal
        public static readonly DependencyProperty HorizontalProperty = DependencyProperty.RegisterAttached(
            "Horizontal"
            , typeof(double)
            , typeof(SpacingAssist)
            , new UIPropertyMetadata(0d, OnHorizontalChanged));

        public static double GetHorizontal(DependencyObject dependencyObject)
        {
            return (double)dependencyObject.GetValue(HorizontalProperty);
        }
        public static void SetHorizontal(DependencyObject dependencyObject, double space)
        {
            dependencyObject.SetValue(HorizontalProperty, space);
        }
        private static void OnHorizontalChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            var space = (double)eventArgs.NewValue;
            var obj = (DependencyObject)sender;

            MarginAssist.SetMargin(obj, new Thickness(0, 0, space, 0));
            MarginAssist.SetLastItemMargin(obj, new Thickness(0));
        }
        #endregion

        #region Vertical
        public static readonly DependencyProperty VerticalProperty = DependencyProperty.RegisterAttached(
            "Vertical"
            , typeof(double)
            , typeof(SpacingAssist)
            , new UIPropertyMetadata(0d, OnVerticalChanged));

        public static double GetVertical(DependencyObject dependencyObject)
        {
            return (double)dependencyObject.GetValue(VerticalProperty);
        }
        public static void SetVertical(DependencyObject dependencyObject, double value)
        {
            dependencyObject.SetValue(VerticalProperty, value);
        }
        private static void OnVerticalChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            var space = (double)eventArgs.NewValue;
            var obj = (DependencyObject)sender;
            MarginAssist.SetMargin(obj, new Thickness(0, 0, 0, space));
            MarginAssist.SetLastItemMargin(obj, new Thickness(0));
        }
        #endregion
    }
}