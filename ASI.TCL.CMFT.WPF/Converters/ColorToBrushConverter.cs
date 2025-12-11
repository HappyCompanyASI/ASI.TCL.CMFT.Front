using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ASI.TCL.CMFT.WPF.Converters
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToBrushConverter : ValueConverterBase<ColorToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                SolidColorBrush rv = new SolidColorBrush(color);
                rv.Freeze();
                return rv;
            }
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                return brush.Color;
            }
            return default(Color);
        }
    }
}