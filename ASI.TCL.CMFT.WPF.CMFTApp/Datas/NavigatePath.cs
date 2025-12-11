using ASI.TCL.CMFT.WPF.Module.DMD.Views;
using ASI.TCL.CMFT.WPF.Module.PA.Views;
using ASI.TCL.CMFT.WPF.CMFTApp.Views;
using ASI.TCL.CMFT.WPF.Module.Alarm.Views;
using ASI.TCL.CMFT.WPF.Module.CCTV.Views;
using ASI.TCL.CMFT.WPF.Module.OTCS.Views;
using ASI.TCL.CMFT.WPF.Module.SYS.Views;
using ASI.TCL.CMFT.WPF.Module.Tetra.Views;

namespace ASI.TCL.CMFT.WPF.CMFTApp.Datas
{
    public static class NavigatePath
    {
        public const string Main = nameof(MainView);

        public const string PAMain = nameof(PAMainView);
        public const string DMDMain = nameof(DMDMainView);
        public const string CCTVMain = nameof(CCTVMainView);
        public const string TetraMain = nameof(TetraMainView);
        public const string OTCSMain = nameof(OTCSMainView);

        public const string PASettingMain = nameof(PASettingMainView);
        public const string DMDSettingMain = nameof(DMDSettingMainView);
        public const string AlarmSettingMain = nameof(AlarmSettingMainView);
        public const string SystemSettingMain = nameof(SystemSettingMainView);
        public const string UserSettingMain = nameof(UserSettingMainView);

        public const string SystemAlarmMain = nameof(SystemAlarmMainView); // 沒有使用
        public const string EventAlarmMain = nameof(EventAlarmMainView);
        public const string EquipAlarmMain = nameof(EquipAlarmMainView);
        public const string StateAndLogMain = nameof(StateAndLogMainView);
    }
}
