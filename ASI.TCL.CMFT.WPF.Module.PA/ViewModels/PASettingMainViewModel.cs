using System;
using ASI.TCL.CMFT.WPF.Module.PA.Datas;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ASI.TCL.CMFT.WPF.Module.PA.ViewModels
{
    public class PASettingMainViewModel : BindableBase, IRegionMemberLifetime, INavigationAware
    {
        public bool KeepAlive => false;

        private readonly IRegionManager _regionManager;

        #region Constructors
        public PASettingMainViewModel()
        {
        }
        public PASettingMainViewModel(IRegionManager regionManager)
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
            _regionManager.RequestNavigate(RegionNames.PASettingsRegion, navigationPath);
        }
        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext context)
        {
            ExecuteNavigateCommand(NavigatePath.PAPreRecordedAudioSettings);
        }

        public bool IsNavigationTarget(NavigationContext context) => true;

        public void OnNavigatedFrom(NavigationContext context)
        {
        }
        #endregion
    }
}