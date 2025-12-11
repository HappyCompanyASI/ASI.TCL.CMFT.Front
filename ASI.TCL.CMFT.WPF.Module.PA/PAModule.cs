using ASI.TCL.CMFT.WPF.Module.PA.ViewModels;
using ASI.TCL.CMFT.WPF.Module.PA.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ASI.TCL.CMFT.WPF.Module.PA
{
    public class PAModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PAPreRecordedAudioSettingsView>();
            containerRegistry.RegisterForNavigation<PAScheduleTemplateSettingsView>();
            containerRegistry.RegisterForNavigation<PAScheduleSettingsView>();
            containerRegistry.RegisterForNavigation<PABroadcastEquipmentSettingsView>();
            containerRegistry.RegisterForNavigation<PABroadcastTimeAdjustSettingsView>();

            containerRegistry.RegisterDialog<PAVoiceGroupSettingsDialogView, PAVoiceGroupSettingsDialogViewModel>();

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}
