using System;
using System.Globalization;
using System.Windows.Controls;

namespace ASI.TCL.CMFT.WPF.Converters
{
    public class RowToIndexConverter : ValueConverterBase<RowToIndexConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DataGridRow dataGridRow)
            {
                return dataGridRow.GetIndex() + 1;
            }
            return -1;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}