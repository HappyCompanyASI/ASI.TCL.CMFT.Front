using System;
using System.Collections;
using System.Globalization;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class KeyToValueConverter : ValueConverterBase<KeyToValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 確保值是字典，參數是有效的鍵
            if (value is IDictionary dictionary && parameter != null)
            {
                if (dictionary.Contains(parameter))
                {
                    return dictionary[parameter]; // 返回字典中對應鍵的值
                }
            }

            return null; // 如果無效，返回 null
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // 不支持反向轉換
        }
    }
}
//KeyToValueMultiConverter