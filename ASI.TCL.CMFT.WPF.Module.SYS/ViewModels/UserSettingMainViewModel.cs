using System;
using ASI.TCL.CMFT.WPF.Module.SYS.Datas;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.SYS.ViewModels
{
    public class UserSettingMainViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        public bool KeepAlive => false;

        private readonly IRegionManager _regionManager;

        #region Constructors
        public UserSettingMainViewModel()
        {
        }
        public UserSettingMainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        #endregion

        #region DelegateCommands
        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(ExecuteNavigateCommand);
        private void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException(nameof(navigationPath), @"導航失敗");
            _regionManager.RequestNavigate(RegionNames.UserSettingsRegion, navigationPath);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext context)
        {
            ExecuteNavigateCommand(NavigatePath.SYSUserRoleSettings);
        }

        public bool IsNavigationTarget(NavigationContext context) => true;

        public void OnNavigatedFrom(NavigationContext context)
        {
        }
        #endregion
    }
}