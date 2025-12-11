using System;
using System.Globalization;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class EqualityToBoolConverter : ValueConverterBase<EqualityToBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
}