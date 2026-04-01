using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSConsoleSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
    }
}
