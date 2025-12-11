using ASI.TCL.CMFT.WPF.Module.SYS.ViewModels;
using ASI.TCL.CMFT.WPF.Module.SYS.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ASI.TCL.CMFT.WPF.Module.SYS
{
    public class SYSModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 使用者
            containerRegistry.RegisterForNavigation<SYSUserRoleSettingsView>();
            containerRegistry.RegisterForNavigation<SYSUserAccountSettingsView>();
            containerRegistry.RegisterForNavigation<SYSUserPasswordSettingsView>();

            // 系統
            containerRegistry.RegisterForNavigation<SYSControlPanelSettingsView>();
            containerRegistry.RegisterForNavigation<SYSRegionalResponsibilitiesSettingsView>();
            containerRegistry.RegisterForNavigation<SYSGroupPhonebookSettingsView>();
            containerRegistry.RegisterForNavigation<SYSInappropriateWordSettingsView>();

            // 狀態紀錄
            containerRegistry.RegisterForNavigation<SYSControlPanelStatusView>();
            containerRegistry.RegisterForNavigation<SYSOperationRecordsView>();

            // 新增帳號
            containerRegistry.RegisterDialog<SYSUserAccountSettingsDialogView, SYSUserAccountSettingsDialogViewModel>();
            // 新增群組
            containerRegistry.RegisterDialog<SYSGroupSettingsDialogView, SYSGroupSettingsDialogViewModel>();
            // 新增不雅字設定
            containerRegistry.RegisterDialog<SYSInappropriateWordSettingsDialogView, SYSInappropriateWordSettingsDialogViewModel>();

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}
