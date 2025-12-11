using System;
using System.Globalization;
using System.Linq;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class MultiValueEqualityConverter : MultiValueConverterBase<MultiValueEqualityConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = values?.All(o => o?.Equals(values[0]) == true) == true || values?.All(o => o == null) == true;
            return result;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}