using System.Windows;
using System.Windows.Forms.Integration;

namespace ASI.TCL.CMFT.WPF.CustomControl
{
    public class WindowsFormsHostControl : WindowsFormsHost
    {
        public static readonly DependencyProperty ContentControlProperty =
            DependencyProperty.Register("ContentControl",
                typeof(System.Windows.Forms.Control),
                typeof(WindowsFormsHostControl),
                new PropertyMetadata(new System.Windows.Forms.Control(),
                    PropertyChaged));

        public static void SetContentControl(UIElement element, string value)
        {
            element.SetValue(ContentControlProperty, value);
        }

        public static string GetContentControl(UIElement element)
        {
            return (string)element.GetValue(ContentControlProperty);
        }

        private static void PropertyChaged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((WindowsFormsHostControl)dependencyObject).Child = (System.Windows.Forms.Control)e.NewValue;
        }
    }
}