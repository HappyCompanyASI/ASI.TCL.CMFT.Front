using ASI.TCL.CMFT.WPF.Module.Alarm.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ASI.TCL.CMFT.WPF.Module.Alarm
{
    public class AlarmsModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AlarmLevelDefinitionSettingsView>();
            containerRegistry.RegisterForNavigation<AlarmTypeDefinitionSettingsView>();

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }
    }
}
