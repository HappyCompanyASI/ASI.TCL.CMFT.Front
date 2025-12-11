using System;
using System.Globalization;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class BoolToConfirmStringConverter : ValueConverterBase<BoolToConfirmStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool? )
            {
                bool? bValue = (bool?)value;
                switch (bValue)
                {
                    case true:
                        return "已確認";
                    case false:
                        return "確認警報";
                    case null:
                        return "確認中";
                }
            }
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}