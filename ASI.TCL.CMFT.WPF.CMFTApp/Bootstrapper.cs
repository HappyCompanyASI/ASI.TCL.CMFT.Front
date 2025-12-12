using System;
using System.Net.Http;
using System.Windows;

using ASI.TCL.CMFT.WPF.CMFTApp.Datas;
using ASI.TCL.CMFT.WPF.CMFTApp.Views;
using ASI.TCL.CMFT.WPF.Exceptions;
using ASI.TCL.CMFT.WPF.Logger;
using ASI.TCL.CMFT.WPF.Module.Alarm.Views;
using ASI.TCL.CMFT.WPF.Module.Auth.Views;
using ASI.TCL.CMFT.WPF.Module.CCTV.Views;
using ASI.TCL.CMFT.WPF.Module.DLTS.Views;
using ASI.TCL.CMFT.WPF.Module.DMD.Views;
using ASI.TCL.CMFT.WPF.Module.OTCS.Views;
using ASI.TCL.CMFT.WPF.Module.PA.Views;
using ASI.TCL.CMFT.WPF.Module.SYS.Views;
using ASI.TCL.CMFT.WPF.Module.Tetra.Views;
using ASI.TCL.CMFT.WPF.Web;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace ASI.TCL.CMFT.WPF.CMFTApp
{
    internal class Bootstrapper : PrismBootstrapper
    {
        /// <summary>
        /// 註冊所有此應用程式所需要的實體
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IExceptionHandlingService, ExceptionHandlingService>();
            Container.Resolve<IExceptionHandlingService>().Initialize(System.Windows.Application.Current);


            containerRegistry.Register<ILogService, SerilogService>();
            var http = new HttpClient();
            http.BaseAddress = new Uri("http://localhost:5278");
            containerRegistry.RegisterInstance<HttpClient>(http);
            containerRegistry.RegisterSingleton<IAuthService, AuthService>();
            containerRegistry.Register<IApiClient,ApiClient>();

            containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<PATrainView>();
            containerRegistry.RegisterForNavigation<PAStationView>();
            containerRegistry.RegisterForNavigation<DMDTrainView>();
            containerRegistry.RegisterForNavigation<DMDStationView>();
            containerRegistry.RegisterForNavigation<TetraDialView>();
            containerRegistry.RegisterForNavigation<TetraGroupCallView>();
            containerRegistry.RegisterForNavigation<TetraSingleCallView>();
            containerRegistry.RegisterForNavigation<OTCSMainView>();
            containerRegistry.RegisterForNavigation<PASettingMainView>();
            containerRegistry.RegisterForNavigation<DMDSettingMainView>();
            containerRegistry.RegisterForNavigation<AlarmSettingMainView>();
            containerRegistry.RegisterForNavigation<SystemSettingMainView>();
            containerRegistry.RegisterForNavigation<UserSettingMainView>();
            containerRegistry.RegisterForNavigation<SystemAlarmMainView>(); // 沒有使用
            containerRegistry.RegisterForNavigation<EquipAlarmMainView>();
            containerRegistry.RegisterForNavigation<EventAlarmMainView>();
            containerRegistry.RegisterForNavigation<StateAndLogMainView>();
        }
        /// <summary>
        /// 解析主畫面
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell() => Container.Resolve<MainWindow>();

        /// <summary>
        /// 加入模組
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog
                .AddModule<Module.Alarm.AlarmsModule>()
                .AddModule<Module.Auth.AuthModule>()
                .AddModule<Module.CCTV.CCTVModule>()
                .AddModule<Module.DLTS.DLTSModule>()
                .AddModule<Module.DMD.DMDModule>()
                .AddModule<Module.OTCS.OTCSModule>()
                .AddModule<Module.PA.PAModule>()
                .AddModule<Module.SYS.SYSModule>()
                .AddModule<Module.Tetra.TetraModule>();
        }

        /// <summary>
        /// 初始化應用程式
        /// </summary>
        protected override async void OnInitialized()
        {
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(MainView));
            RegisterViews(regionManager);
            base.OnInitialized();
        }

        /// <summary>
        /// 初始化應用程式  
        /// </summary>
        private void RegisterViews(IRegionManager regionManager)
        {
            //系統功能
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(PAMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(PATrainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(PAStationView));
           
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(DMDMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(DMDTrainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(DMDStationView));

            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(TetraMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(TetraDialView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(TetraGroupCallView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(TetraSingleCallView));

            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(OTCSMainView));

            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(CCTVMainView));

            regionManager.RegisterViewWithRegion(RegionNames.InformationRegion, typeof(UserInformationView));
            regionManager.RegisterViewWithRegion(RegionNames.DLTSContentRegion, typeof(DLTSPanelView));
            regionManager.RegisterViewWithRegion(RegionNames.TetraLiveCallRegion, typeof(TetraLiveCallPanelView));
            regionManager.RegisterViewWithRegion(RegionNames.TetraCallListRegion, typeof(TetraCallListPanelView));
            regionManager.RegisterViewWithRegion(RegionNames.OTCSCallListRegion, typeof(OTCSCallListPanelView));
            regionManager.RegisterViewWithRegion(RegionNames.StationTrackMapRegion, typeof(StationTrackMapView));

            //系統設定
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(PASettingMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(DMDSettingMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(AlarmSettingMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(SystemSettingMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(UserSettingMainView));

            //系統狀態
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(SystemAlarmMainView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(StateAndLogMainView));
        }
    }
}