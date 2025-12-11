namespace ASI.TCL.CMFT.WPF.Applications
{
    public interface IConfigurationService<T> where T : class, IAppSettings, new()
    {
        /// <summary>
        /// 獲取整體設定物件。
        /// </summary>
        /// <returns>完整的設定類型。</returns>
        T GetSettings();

        /// <summary>
        /// 重新加載設定檔。
        /// </summary>
        void ReloadSettings();

        /// <summary>
        /// 儲存整體設定物件。
        /// </summary>
        /// <param name="settings">完整的設定物件。</param>
        void SaveSettings(IAppSettings settings);

        /// <summary>
        /// 獲取子設定物件。
        /// </summary>
        /// <typeparam name="TChild">子設定類型。</typeparam>
        /// <returns>子設定物件。</returns>
        TChild GetChildSettings<TChild>() where TChild : class, IAppSettings;

        /// <summary>
        /// 儲存子設定物件。
        /// </summary>
        /// <typeparam name="TChild">子設定類型。</typeparam>
        /// <param name="settings">子設定物件。</param>
        void SaveChildSettings<TChild>(TChild settings) where TChild : class, IAppSettings;
    }
}