using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Module.PA.Behaviors
{
    public class ListViewSelectionChangedBehavior : Behavior<ListView>
    {
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ListViewSelectionChangedBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 只允許來源是 ListView 自身（避免子元素觸發）
            if (e.OriginalSource is ListView listView && listView == AssociatedObject)
            {
                if (Command?.CanExecute(null) == true)
                {
                    Command.Execute(null);
                }
            }
        }
    }
}
