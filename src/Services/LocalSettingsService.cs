using BackMeUp.Contracts.Services;
using BackMeUp.Data;
using BackMeUp.Models;
using BackMeUp.Properties;
using System.Linq.Expressions;
using System.Reflection;
using Windows.Storage;

namespace BackMeUp.Services
{
    public class LocalSettingsService(IFileService fileService) : ILocalSettingsService
    {
        private static readonly string ApplicationDataFolder = Path.Combine(ApplicationData.Current.LocalFolder.Path, Resources.AppFolder);
        private static readonly string LocalSettingsFile = Resources.LocalSettingsFile;

        private Settings _settings;
        private bool _isInitialized;

        public async Task<T?> ReadSettingAsync<T>(Expression<Func<Settings, T>> selector)
        {
            await InitializeAsync();

            var compiledSelector = selector.Compile();
            return compiledSelector(_settings);
        }

        public async Task SaveSettingAsync<T>(Expression<Func<Settings, T>> selector, T value)
        {
            await InitializeAsync();

            var memberSelectorExpression = selector.Body as MemberExpression;
            var property = memberSelectorExpression?.Member as PropertyInfo;

            property?.SetValue(_settings, value);

            fileService.Save(ApplicationDataFolder, LocalSettingsFile, _settings);
        }
        
        private async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                _settings = await Task.Run(() => 
                    fileService.Read<Settings>(
                        ApplicationDataFolder, LocalSettingsFile)) ?? DefaultSettings.DefaultSettingsData;
                _isInitialized = true;
            }
        }
    }
}
