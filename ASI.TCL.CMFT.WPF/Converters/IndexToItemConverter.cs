using System;
using System.Collections;
using System.Globalization;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class IndexToItemConverter :  ValueConverterBase<IndexToItemConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 確保值是集合且參數是有效的索引
            if (value is IList list && int.TryParse(parameter?.ToString(), out int index))
            {
                if (index >= 0 && index < list.Count)
                    return list[index]; // 返回集合中的指定項目
            }

            return null; // 如果無效，返回 null
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}