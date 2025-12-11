using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class DistinctConverter : ValueConverterBase<DistinctConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable values)
                return values.Cast<object>().Distinct();
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}