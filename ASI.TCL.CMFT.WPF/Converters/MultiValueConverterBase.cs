using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public abstract class MultiValueConverterBase<ConverterType> : MarkupExtension, IMultiValueConverter where ConverterType : class, new()
    {
        public abstract object Convert(object[] value, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture);

        public static readonly ConverterType Instance = new ConverterType();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
    }
}