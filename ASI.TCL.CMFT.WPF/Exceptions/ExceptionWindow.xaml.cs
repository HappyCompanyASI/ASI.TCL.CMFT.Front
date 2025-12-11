using System.Windows;
using ASI.TCL.CMFT.WPF.Utilities;

namespace ASI.TCL.CMFT.WPF.Exceptions
{
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow()
        {
            InitializeComponent();
            ScreenSetting.SetScreen(true, this);
        }
    }
}