using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace ASI.TCL.CMFT.WPF.TriggerActions
{
    public class SelectAllTriggerAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty  TargetProperty = DependencyProperty.RegisterAttached(
            "Target"
            , typeof(object)
            , typeof(SelectAllTriggerAction)
            , new PropertyMetadata(null));

        public static object GetTarget(DependencyObject obj) => (object)obj.GetValue(TargetProperty);
        public static void SetTarget(DependencyObject obj, int value) => obj.SetValue(TargetProperty, value);
        
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is ListBox ListBoxSource ) 
                ListBoxSource.Loaded += ListBoxSource_Loaded;
        }
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is ListBox listBoxSource && GetTarget(this) is CheckBox checkBoxTarget)
                ChangeTarget(listBoxSource, checkBoxTarget);
            else if (AssociatedObject is CheckBox checkBoxSource && GetTarget(this) is ListBox listBoxTarget) 
                ChangeTarget(checkBoxSource, listBoxTarget);
        }

        private void ListBoxSource_Loaded(object sender, RoutedEventArgs e)
        {
            //讓畫面初始時,依照listbox當時的選擇  設定全選的checkbox 
            if (AssociatedObject is ListBox listBoxSource && GetTarget(this) is CheckBox checkBoxTarget)
                ChangeTarget(listBoxSource, checkBoxTarget);
        }
        private void ChangeTarget(ListBox source, CheckBox target)
        {
            var ItemsCount = source.Items.Count;
            var selectedCount = source.SelectedItems.Count;

            if (selectedCount == 0)
                target.IsChecked = false;
            else if (selectedCount == ItemsCount)
                target.IsChecked = true;
            else
                target.IsChecked = null;
        }
        private void ChangeTarget(CheckBox source, ListBox target)
        {
            if (source.IsChecked == true)
                target.SelectAll();
            else
                target.UnselectAll();
        }
    }
}