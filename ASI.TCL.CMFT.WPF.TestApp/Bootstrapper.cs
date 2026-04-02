using System.Windows;

using Prism.Ioc;
using Prism.Unity;

namespace ASI.TCL.CMFT.WPF.TestApp
{
    internal class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}