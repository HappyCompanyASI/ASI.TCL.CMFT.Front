using System;
using System.Globalization;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class MultiValueToVisibilityConverter :  MultiValueConverterBase<MultiValueToVisibilityConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] != null && values[1] != null)
            {
                string selectedItemGroupName = values[0].ToString();
                string tagValue = values[1].ToString();

                return selectedItemGroupName == tagValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}