using System.Windows;

namespace ASI.TCL.CMFT.WPF.CMFTApp
{
    public partial class App
    {
        #region Start/Exit
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new Bootstrapper().Run();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
        #endregion
    }
}
