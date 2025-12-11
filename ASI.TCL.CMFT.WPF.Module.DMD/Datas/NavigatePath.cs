using ASI.TCL.CMFT.WPF.Module.DMD.Views;

namespace ASI.TCL.CMFT.WPF.Module.DMD.Datas
{
    public static class NavigatePath
    {
        //DMD功能頁面
        public const string DMDMain = nameof(DMDMainView);
        public const string DMDTrain = nameof(DMDTrainView);
        public const string DMDStation = nameof(DMDStationView);

        //DMD設定頁面
        public const string DMDSettingMain = nameof(DMDSettingMainView);
        public const string DMDPreRecordedMessageSettings = nameof(DMDPreRecordedMessageSettingsView);
        public const string DMDScheduleTemplateSettings = nameof(DMDScheduleTemplateSettingsView);
        public const string DMDScheduleSettings = nameof(DMDScheduleSettingsView);
        public const string DMDTrainMessageSettings = nameof(DMDTrainMessageSettingsView);
        public const string DMDDisplayModeSettings = nameof(DMDDisplayModeSettingsView);
    }
}
