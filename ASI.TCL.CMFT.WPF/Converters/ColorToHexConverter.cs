using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;
using ASI.TCL.CMFT.WPF.Utilities;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class ColorToHexConverter : ValueConverterBase<ColorToHexConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color ) 
            {
                return color.ColorToHex();
            }
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && Regex.IsMatch(str, @"[#][0-9A-Fa-f]{6}\b"))
            {
                return value;
            }
            return Binding.DoNothing;
        }
    }
    public class HexToColorConverter : ValueConverterBase<HexToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && Regex.IsMatch(str, @"[#][0-9A-Fa-f]{6}\b"))
            {
                return str.HexToColor();
            }
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return color.ColorToHex();
            }
            return Binding.DoNothing;
        }
    }
}