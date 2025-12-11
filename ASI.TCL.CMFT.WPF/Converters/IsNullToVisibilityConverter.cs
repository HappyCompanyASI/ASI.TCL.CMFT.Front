using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class IsNullToVisibilityConverter : ValueConverterBase<IsNullToVisibilityConverter>
    {
        public bool Invert { get; set; }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invert)
            {
                return value is null ? Visibility.Visible : Visibility.Collapsed;
            }
            return value is null ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}