using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ASI.TCL.CMFT.WPF.Converters
{
    //直接利用擴充標記，無須在放入Resources了
//ex:
//xmlns:converters="clr-namespace:ASI.TCL.CMFT.WPF.Common.Converters"
//Converter={converters:StationIDToStationNameConverter}}"
    public abstract class ValueConverterBase<ConverterType> : MarkupExtension, IValueConverter where ConverterType : class, new()
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        public static readonly ConverterType Instance = new ConverterType();
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }


        //不要使用靜態
        //避免使用靜態的 Instance，這樣每次 XAML 中使用該轉換器時都會創建一個新的實例，而不是重用同一個靜態實例。
        //當你使用像 BooleanToVisibilityConverter 這樣的轉換器時，WPF 會在第一次需要轉換時調用 ProvideValue，並使用靜態的 Instance，但如果在 XAML 中你試圖設置 Invert 屬性，這些屬性值可能沒有在正確的實例上應用，因為你使用了靜態的 Instance，而靜態實例不會被 WPF 監測屬性改變。
        //ex: BooleanToVisibilityConverter 中的 Invert 屬性之所以在畫面一開始的時候無效，是因為在 XAML 中使用該轉換器時，Invert 屬性沒有正確地初始化。

    }
}