using System;
using System.Globalization;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class BoolNegationConverter : ValueConverterBase<BoolNegationConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bValue )
            {
                return !bValue;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}