using System.ComponentModel;
using System.Windows;

namespace ASI.TCL.CMFT.WPF.Utilities
{
    public static class WPFHelper
    {
        public static bool IsDesignTime = DesignerProperties.GetIsInDesignMode(new DependencyObject());
    }
}