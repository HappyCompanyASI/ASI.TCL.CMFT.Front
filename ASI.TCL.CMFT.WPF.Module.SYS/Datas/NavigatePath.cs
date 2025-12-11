using ASI.TCL.CMFT.WPF.Module.SYS.Views;

namespace ASI.TCL.CMFT.WPF.Module.SYS.Datas
{
    public static class NavigatePath
    {
        /// <summary>
        /// 使用者設定
        /// </summary>
        
        /// 使用者角色
        public const string SYSUserRoleSettings = nameof(SYSUserRoleSettingsView);
        /// 使用者帳號
        public const string SYSUserAccountSettings = nameof(SYSUserAccountSettingsView);
        /// 使用者密碼
        public const string SYSUserPasswordSettings = nameof(SYSUserPasswordSettingsView);


        /// <summary>
        /// 系統設定
        /// </summary>

        /// 主控台設定
        public const string SYSControlPanelSettings = nameof(SYSControlPanelSettingsView);
        // 區域權責設定
        public const string SYSRegionalResponsibilitiesSettings = nameof(SYSRegionalResponsibilitiesSettingsView);
        // 群組與電話簿
        public const string SYSGroupPhonebookSettings = nameof(SYSGroupPhonebookSettingsView);
        // 不雅字設定
        public const string SYSInappropriateWordSettings = nameof(SYSInappropriateWordSettingsView);

        /// <summary>
        /// 狀態與紀錄
        /// </summary>

        /// 主控台狀態
        public const string SYSControlPanelStatus = nameof(SYSControlPanelStatusView);
        // 操作紀錄
        public const string SYSOperationRecords = nameof(SYSOperationRecordsView);

    }
}
