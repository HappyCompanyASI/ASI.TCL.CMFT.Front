using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using ASI.TCL.CMFT.WPF.Utilities;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class HexToBrushConverter : ValueConverterBase<BrushToHexConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && Regex.IsMatch(str, @"[#][0-9A-Fa-f]{6}\b"))
            {
                return str.HexToBrush();
            }
            return new SolidColorBrush();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}