using System;
using System.Globalization;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class IsStringNullOrEmptyConverter : ValueConverterBase<IsStringNullOrEmptyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}