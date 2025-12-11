using System.Collections;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace ASI.TCL.CMFT.WPF.TriggerActions
{
    public class DoNothingTriggerAction : TriggerAction<FrameworkElement>
    {
       
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached(
            "Source"
            , typeof(IList)
            , typeof(DoNothingTriggerAction)
            , new PropertyMetadata(null));

        public static IList GetSource(DependencyObject obj) => (IList)obj.GetValue(SourceProperty);
        public static void SetSource(DependencyObject obj, int value) => obj.SetValue(SourceProperty, value);

        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target"
            , typeof(IList)
            , typeof(DoNothingTriggerAction)
            , new PropertyMetadata(null));

        public static IList GetTarget(DependencyObject obj) => (IList)obj.GetValue(TargetProperty);
        public static void SetTarget(DependencyObject obj, int value) => obj.SetValue(TargetProperty, value);


        public static readonly DependencyProperty TargetOperationProperty = DependencyProperty.RegisterAttached(
            "TargetOperation"
            , typeof(string)
            , typeof(DoNothingTriggerAction)
            , new PropertyMetadata(null));

        public static string GetTargetOperation(DependencyObject obj) => (string)obj.GetValue(TargetOperationProperty);
        public static void SetTargetOperation(DependencyObject obj, string value) => obj.SetValue(TargetOperationProperty, value);

        public static readonly DependencyProperty OperationIDProperty = DependencyProperty.RegisterAttached(
            "OperationID"
            , typeof(int)
            , typeof(DoNothingTriggerAction)
            , new PropertyMetadata(null));

        public static int GetOperationID(DependencyObject obj) => (int)obj.GetValue(OperationIDProperty);
        public static void SetOperationID(DependencyObject obj, int value) => obj.SetValue(OperationIDProperty, value);

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (args, e) =>
            {
                if (!(AssociatedObject is FrameworkElement source) || !(GetTarget(this) is FrameworkElement target))
                    return;
            };
        }

        protected override void Invoke(object parameter)
        {
            var TargetOperation = GetTargetOperation(this);
            var SourceList = GetSource(this);
            var TargetList = GetTarget(this);
            var OperationID = GetOperationID(this);

            if (SourceList == null || TargetList == null || string.IsNullOrEmpty(TargetOperation))
                return;
            //Debugger.Break();

            switch (TargetOperation)
            {
                case "Add":
                    foreach (var SourceItem in SourceList)
                    {
                        if(!TargetList.Contains(SourceItem))
                        {
                            TargetList.Add(SourceItem);
                        }
                    }
                    break;

                case "Remove":
                    foreach (var SourceItem in SourceList)
                    {
                        if (TargetList.Contains(SourceItem))
                        {
                            TargetList.Remove(SourceItem);
                        }
                    }
                    break;
            }
        }
    }
}