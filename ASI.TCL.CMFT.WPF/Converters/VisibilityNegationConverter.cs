using System;
using System.Globalization;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class VisibilityNegationConverter : ValueConverterBase<VisibilityNegationConverter>
    {
        public override object  Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue)
            {
                switch (visibilityValue)
                {
                    case Visibility.Visible:
                        return Visibility.Collapsed;
                    case Visibility.Collapsed:
                        return Visibility.Visible;
                }
            }
            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}