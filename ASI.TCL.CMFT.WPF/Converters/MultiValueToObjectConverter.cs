using System;
using System.Globalization;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class MultiValueToObjectConverter : MultiValueConverterBase<MultiValueToObjectConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}