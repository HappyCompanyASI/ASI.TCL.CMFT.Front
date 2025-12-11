using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    internal class ExpanderRotateAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double factor = 1.0;
            var parameterValue = parameter;

            if (parameterValue != null)
            {
                if (!double.TryParse(parameterValue.ToString(), out factor))
                {
                    factor = 1.0;
                }

            }
            var returnValue = 0.0;
            switch (value)
            {
                case ExpandDirection.Left:
                    returnValue = 90 * factor;
                    return returnValue;
                case ExpandDirection.Right:
                    returnValue = -90 * factor;
                    return returnValue;
                default: return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}