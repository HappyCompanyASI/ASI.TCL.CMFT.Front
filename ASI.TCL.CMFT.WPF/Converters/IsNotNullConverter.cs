using System;
using System.Globalization;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class IsNotNullConverter : ValueConverterBase<IsNotNullConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is null);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}