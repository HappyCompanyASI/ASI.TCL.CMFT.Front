using System.Windows;
using System.Windows.Controls;

namespace ASI.TCL.CMFT.WPF.Assist
{
    public static class PasswordBoxAssist
    {
        #region BindPassword
        public static readonly DependencyProperty BindPasswordProperty = DependencyProperty.RegisterAttached(
            "BindPassword"
            , typeof(bool)
            , typeof(PasswordBoxAssist)
            , new PropertyMetadata(false, OnBindPasswordChanged));

        public static bool GetBindPassword(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(BindPasswordProperty);
        }
        public static void SetBindPassword(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(BindPasswordProperty, value);
        }
        private static void OnBindPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            PasswordBox box = dependencyObject as PasswordBox;

            if (box == null)
            {
                return;
            }

            bool wasBound = (bool)eventArgs.OldValue;
            bool needToBind = (bool)eventArgs.NewValue;

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }
        #endregion

        #region BoundPassword
        public static readonly DependencyProperty BoundPasswordProperty = DependencyProperty.RegisterAttached(
            "BoundPassword"
            , typeof(string)
            , typeof(PasswordBoxAssist)
            , new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BoundPasswordProperty);
        }
        public static void SetBoundPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(BoundPasswordProperty, value);
        }
        private static void OnBoundPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            PasswordBox box = dependencyObject as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (dependencyObject == null || !GetBindPassword(dependencyObject))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)eventArgs.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }
        #endregion

        #region UpdatingPassword
        private static readonly DependencyProperty UpdatingPasswordProperty = DependencyProperty.RegisterAttached(
            "UpdatingPassword"
            , typeof(bool)
            , typeof(PasswordBoxAssist)
            , new PropertyMetadata(false));

        private static bool GetUpdatingPassword(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(UpdatingPasswordProperty);
        }
        private static void SetUpdatingPassword(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(UpdatingPasswordProperty, value);
        }
        #endregion

        private static void HandlePasswordChanged(object sender, RoutedEventArgs eventArgs)
        {
            PasswordBox box = sender as PasswordBox;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }

    }
}