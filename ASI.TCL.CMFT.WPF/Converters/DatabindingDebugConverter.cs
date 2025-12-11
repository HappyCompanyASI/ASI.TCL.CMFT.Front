using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class DatabindingDebugConverter : ValueConverterBase<DatabindingDebugConverter>
    {
        public override object Convert(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            if (value == null) return Binding.DoNothing;

            var type = value.GetType();
            Debug.Print(value.ToString());

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Binding.DoNothing;

            Debug.Print(value.ToString());

            return Binding.DoNothing;
        }
    }
}