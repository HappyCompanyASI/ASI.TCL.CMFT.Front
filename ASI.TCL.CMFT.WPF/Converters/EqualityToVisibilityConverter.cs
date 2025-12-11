using System;
using System.Globalization;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class EqualityToVisibilityConverter : ValueConverterBase<EqualityToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.Equals(parameter))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}