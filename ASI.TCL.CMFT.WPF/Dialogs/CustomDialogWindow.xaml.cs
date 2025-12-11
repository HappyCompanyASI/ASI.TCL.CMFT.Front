using System.Windows;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF.Dialogs
{
    /// <summary>
    /// CustomDialogWindow.xaml 的互動邏輯
    /// </summary>
    public partial class CustomDialogWindow : Window, IDialogWindow
    {
        public IDialogResult Result { get; set; }
        public CustomDialogWindow()
        {
            InitializeComponent();
        }
    }
}