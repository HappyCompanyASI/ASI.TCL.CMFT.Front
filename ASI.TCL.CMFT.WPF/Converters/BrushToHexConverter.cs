using System;
using System.Globalization;
using System.Windows.Media;
using ASI.TCL.CMFT.WPF.Utilities;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class BrushToHexConverter : ValueConverterBase<BrushToHexConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                return brush.BrushToHex();
            }
            return string.Empty;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}