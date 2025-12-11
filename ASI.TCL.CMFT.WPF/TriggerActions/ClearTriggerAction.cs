using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors;

namespace ASI.TCL.CMFT.WPF.TriggerActions
{
    public class ClearTriggerAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target"
            , typeof(object)
            , typeof(ClearTriggerAction)
            , new PropertyMetadata(null));

        public static object GetTarget(DependencyObject obj) => (object)obj.GetValue(TargetProperty);

        public static void SetTarget(DependencyObject obj, int value) => obj.SetValue(TargetProperty, value);

        protected override void Invoke(object parameter)
        {
            var target = GetTarget(this);

            switch (target)
            {
                case TextBox textBox:
                    textBox.SetCurrentValue(TextBox.TextProperty, null);
                    break;
                case ComboBox comboBox:
                    comboBox.SetCurrentValue(ComboBox.TextProperty, null);
                    comboBox.SetCurrentValue(Selector.SelectedItemProperty, null);
                    break;
                case RichTextBox richTextBox:
                    richTextBox.Document.Blocks.Clear();
                    break;
                case PasswordBox passwordBox:
                    passwordBox.Password = null;
                    break;
                case DatePicker datePicker:
                    datePicker.SetCurrentValue(DatePicker.SelectedDateProperty, null);
                    datePicker.Text = string.Empty; // Clears the text in the DatePickerTextBox which could contain uncommitted text
                    break;
                case TimePicker timePicker:
                    timePicker.SetCurrentValue(TimePicker.SelectedTimeProperty, null);
                    timePicker.Text = string.Empty; // Clears the text in the TimePickerTextBox which could contain uncommitted text
                    break;
                case ListBox listBox:
                    listBox.UnselectAll();
                    break;
                case CheckBox checkBox:
                    checkBox.IsChecked = false;
                    break;

            }
        }
    }
}