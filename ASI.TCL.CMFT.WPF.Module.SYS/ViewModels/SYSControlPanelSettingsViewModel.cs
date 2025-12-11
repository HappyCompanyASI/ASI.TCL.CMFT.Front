using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class SYSControlPanelSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
    }
}
