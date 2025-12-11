using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ASI.TCL.CMFT.WPF.Applications
{
    public class ConfigurationService<T> : IConfigurationService<T> where T : class, IAppSettings, new()
    {
        private readonly string _configFilePath; // 執行檔的設定檔的相對路徑
        private T _settings;

        public ConfigurationService(string configFilePath)
        {
            _configFilePath = configFilePath;
            LoadSettings();
        }
        public T GetSettings() => _settings;
        public void ReloadSettings() => LoadSettings();
        public void SaveSettings(IAppSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            try
            {
                var json = File.Exists(_configFilePath) ? File.ReadAllText(_configFilePath) : "{}";

                var existingSettings = JsonConvert.DeserializeObject<T>(json) ?? new T();

                if (settings.GetType() == typeof(T))
                {
                    _settings = (T)settings;
                }
                else
                {
                    var property = typeof(T).GetProperties()
                        .FirstOrDefault(p => p.PropertyType == settings.GetType());

                    if (property == null)
                    {
                        throw new InvalidOperationException($"No matching property found for type {settings.GetType().Name}");
                    }

                    property.SetValue(existingSettings, settings);
                    _settings = existingSettings;
                }

                var updatedJson = JsonConvert.SerializeObject(_settings, Formatting.Indented);
                File.WriteAllText(_configFilePath, updatedJson);

                CopyExeConfigToProjectConfig();
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save settings to {_configFilePath}: {ex.Message}", ex);
            }
        }
        public TChild GetChildSettings<TChild>() where TChild : class, IAppSettings
        {
            if (typeof(TChild) == typeof(T))
            {
                return _settings as TChild;
            }

            var property = typeof(T).GetProperties()
                .FirstOrDefault(p => p.PropertyType == typeof(TChild));

            if (property == null)
            {
                throw new InvalidOperationException($"No matching property found for type {typeof(TChild).Name}");
            }

            return property.GetValue(_settings) as TChild;
        }
        public void SaveChildSettings<TChild>(TChild settings) where TChild : class, IAppSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var property = typeof(T).GetProperties()
                .FirstOrDefault(p => p.PropertyType == typeof(TChild));

            if (property == null)
            {
                throw new InvalidOperationException($"No matching property found for type {typeof(TChild).Name}");
            }

            property.SetValue(_settings, settings);
            SaveSettings(_settings);
        }
        private void LoadSettings()
        {
            if (File.Exists(_configFilePath))
            {
                var json = File.ReadAllText(_configFilePath);

                try
                {
                    _settings = JsonConvert.DeserializeObject<T>(json) ?? new T();

                    if (_settings == null)
                    {
                        throw new InvalidOperationException("Failed to deserialize configuration file.");
                    }
                }
                catch (JsonException ex)
                {
                    throw new InvalidOperationException("Failed to parse configuration file.", ex);
                }
            }
            else
            {
                throw new FileNotFoundException($"Configuration file not found: {_configFilePath}\n Current Directory: {Directory.GetCurrentDirectory()}");
            }
        }



        private void CopyExeConfigToProjectConfig()
        {
            // 執行檔的設定檔 存回 專案資料夾的設定檔

            // 執行檔的設定檔的絕對路徑
            var sourcePath = Path.Combine(GetExePath(), _configFilePath);
            // 專案資料夾的設定檔的絕對路徑
            var destPath = Path.Combine(GetProjectPath(), _configFilePath);

            if (File.Exists(sourcePath))
                File.Copy(sourcePath, destPath, true);
        }

        private string GetExePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        private string GetProjectPath()
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            var dir = new DirectoryInfo(currentDir);

            while (dir != null && !dir.GetFiles("*.csproj").Any())
            {
                dir = dir.Parent;
            }
            return dir != null ? dir.FullName : throw new Exception("找不到專案目錄");
        }
    }
}
