using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using BackMeUp.Properties;
using Windows.Storage;

namespace BackMeUp.Services
{
    public class LocalSettingsService(IFileService fileService) : ILocalSettingsService
    {
        private static readonly string ApplicationDataFolder = Path.Combine(ApplicationData.Current.LocalFolder.Path, Resources.AppFolder);
        private static readonly string LocalSettingsFile = Resources.LocalSettingsFile;

        private IDictionary<string, object> _settings;
        private bool _isInitialized;

        public async Task<T?> ReadSettingAsync<T>(string key)
        {
            await InitializeAsync();

            if (_settings.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
            return default;
        }

        public async Task SaveSettingAsync<T>(string key, T value)
        {
            await InitializeAsync();

            _settings[key] = await Json.StringifyAsync(value!);
            fileService.Save(ApplicationDataFolder, LocalSettingsFile, _settings);
        }
        
        private async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                _settings = await Task.Run(() => 
                    fileService.Read<IDictionary<string, object>>(
                        ApplicationDataFolder, LocalSettingsFile)) ?? new Dictionary<string, object>();
                _isInitialized = true;
            }
        }
    }
}
