using ASI.TCL.CMFT.WPF.Dialogs;
using ASI.TCL.CMFT.WPF.Logger;
using ASI.TCL.CMFT.WPF.ThemeSettings;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace ASI.TCL.CMFT.WPF
{
    public static class BootstrapperExtensions
    {
        public static IContainerRegistry RegisterWPF(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogService, ExtendedPrismDialogService>();
            containerRegistry.Register<IMaterialThemeService, MaterialThemeService>();

            containerRegistry.RegisterSingleton<ILogService, SerilogService>();
            containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>();
            containerRegistry.RegisterDialog<ThemeSettingDialog, ThemeSettingDialogViewModel>();
          
            return containerRegistry;
        }
    }
}
