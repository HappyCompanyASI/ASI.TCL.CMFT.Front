using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class TextBoxAssist
    {
        #region IsNumericOnly
        public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty.RegisterAttached(
            "IsNumericOnly"
            , typeof(bool)
            , typeof(TextBoxAssist)
            , new UIPropertyMetadata(default(bool), OnIsNumericOnlyChanged));

        public static bool GetIsNumericOnly(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsNumericOnlyProperty);
        }
        public static void SetIsNumericOnly(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsNumericOnlyProperty, value);
        }
        public static void OnIsNumericOnlyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is TextBox textBox)
            {
                bool newIsNumericOnlyValue = (bool)eventArgs.NewValue;
                TextCompositionEventHandler textBox_PreviewTextInput = (s, e) => e.Handled = !e.Text.All(c => char.IsNumber(c) && c != ' ');
                KeyEventHandler textBox_PreviewKeyDown = (s, e) => e.Handled = e.Key == Key.Space;
                if (newIsNumericOnlyValue)
                {
                    textBox.PreviewTextInput += textBox_PreviewTextInput;
                    textBox.PreviewKeyDown += textBox_PreviewKeyDown;
                }
                else
                {
                    textBox.PreviewTextInput -= textBox_PreviewTextInput;
                    textBox.PreviewKeyDown -= textBox_PreviewKeyDown;
                }
            }
        }
        #endregion
    }
}