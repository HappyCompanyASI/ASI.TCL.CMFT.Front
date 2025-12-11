using ASI.TCL.CMFT.WPF.Module.DMD.ViewModels;
using ASI.TCL.CMFT.WPF.Module.DMD.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ASI.TCL.CMFT.WPF.Module.DMD
{
    public class DMDModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DMDPreRecordedMessageSettingsView>();
            containerRegistry.RegisterForNavigation<DMDScheduleTemplateSettingsView>();
            containerRegistry.RegisterForNavigation<DMDScheduleSettingsView>();
            containerRegistry.RegisterForNavigation<DMDTrainMessageSettingsView>();
            containerRegistry.RegisterForNavigation<DMDDisplayModeSettingsView>();

            containerRegistry.RegisterDialog<DMDMessageGroupSettingsDialogView, DMDMessageGroupSettingsDialogViewModel>();

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }
    }
}
