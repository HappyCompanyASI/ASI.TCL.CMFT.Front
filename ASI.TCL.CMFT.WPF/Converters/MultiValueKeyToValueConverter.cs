using System;
using System.Collections;
using System.Globalization;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class MultiValueKeyToValueConverter: MultiValueConverterBase<MultiValueKeyToValueConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is IDictionary dictionary && values[1] != null)
            {
                var key = values[1];
                return dictionary.Contains(key) ? dictionary[key] : null;
            }

            return null;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}