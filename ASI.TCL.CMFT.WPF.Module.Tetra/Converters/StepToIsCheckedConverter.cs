using System;
using System.Globalization;
using System.Windows.Data;
using ASI.TCL.CMFT.WPF.Converters;
using ASI.TCL.CMFT.WPF.Module.Tetra.DataTypes;

namespace ASI.TCL.CMFT.WPF.Module.Tetra.Converters
{
    public class StepToIsCheckedConverter : ValueConverterBase<StepToIsCheckedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is eStep currentStep && parameter is eStep step)
            {
                if(currentStep >= step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {   
            return Binding.DoNothing;
        }
    }
}