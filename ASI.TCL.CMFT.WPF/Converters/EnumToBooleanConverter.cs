using System;
using System.Globalization;
using System.Windows.Data;

namespace ASI.TCL.CMFT.WPF.Converters
{
    /// <summary>
    /// 將 Enum 值與 Boolean 互轉，常用於 RadioButton 綁定 Enum 屬性。
    /// ConverterParameter 請傳入 enum 的某個值（通常用 x:Static）。
    /// </summary>
    public class EnumToBooleanConverter : ValueConverterBase<EnumToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            // 取得 enum 的型別
            var enumType = value.GetType();

            // 解析 parameter 成 enum 值
            object paramValue;
            if (parameter is string paramString)
            {
                paramValue = Enum.Parse(enumType, paramString);
            }
            else
            {
                paramValue = parameter;
            }

            return value.Equals(paramValue);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Binding.DoNothing;

            bool isChecked = value as bool? ?? false;
            if (!isChecked)
                return Binding.DoNothing;

            // 解析 parameter 成 enum 值
            if (parameter is string paramString)
            {
                return Enum.Parse(targetType, paramString);
            }
            return parameter;
        }
    }
}