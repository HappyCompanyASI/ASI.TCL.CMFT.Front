using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace ASI.TCL.CMFT.WPF.TriggerActions
{
    //TriggerAction<CheckBox> :AssociatedObject只能是ToggleButton Or  ToggleButton的衍伸類
    public class CheckEnableTriggerAction : TriggerAction<ToggleButton>
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target"
            , typeof(object)
            , typeof(CheckEnableTriggerAction)
            , new PropertyMetadata(null));

        public static object GetTarget(DependencyObject obj) => (object)obj.GetValue(TargetProperty);

        public static void SetTarget(DependencyObject obj, int value) => obj.SetValue(TargetProperty, value);

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (args, e) =>
            {
                if (!(AssociatedObject is CheckBox source) || !(GetTarget(this) is FrameworkElement target)) 
                    return;

                switch (target)
                {
                    case TextBox textBox:
                        if (!string.IsNullOrEmpty(textBox.Text)) 
                            source.IsChecked = true;
                        break;
                    case ComboBox comboBox:
                        if (comboBox.SelectedItem != null) 
                            source.IsChecked = true;
                        break;
                }
                if (source.IsChecked == false)
                {
                    target.IsEnabled = false;
                }
            };
        }

        protected override void Invoke(object parameter)
        {
            if (!(AssociatedObject is CheckBox source) || !(GetTarget(this) is FrameworkElement target))
                return;

            target.SetCurrentValue(UIElement.IsEnabledProperty, source.IsChecked);
            switch (target)
            {
                case TextBox textBox:
                    textBox.SetCurrentValue(TextBox.TextProperty, null);
                    break;
                case ComboBox comboBox:
                    comboBox.SetCurrentValue(Selector.SelectedIndexProperty, source.IsChecked == true ? comboBox.SelectedIndex = 0 : -1);
                    break;
            }
        }
    }
}