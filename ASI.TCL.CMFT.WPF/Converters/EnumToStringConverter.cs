using System;
using System.Globalization;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class EnumToStringConverter : ValueConverterBase<EnumToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                return value.ToString();
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}