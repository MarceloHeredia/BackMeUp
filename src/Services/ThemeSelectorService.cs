using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using BackMeUp.Models;
using BackMeUp.Properties;
using Microsoft.UI.Xaml;


namespace BackMeUp.Services;

public class ThemeSelectorService(IApplicationService applicationService, ILocalSettingsService localSettingsService) : IThemeSelectorService
{
    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    private static readonly string SettingsKey = Resources.ThemeSettingsKey;

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (applicationService.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme, (applicationService.MainWindow as MainWindow)!);
        }

        await Task.CompletedTask;
    }

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await localSettingsService.ReadSettingAsync<string>(s => s.AppBackgroundRequestedTheme);

        var someTesat = await localSettingsService.ReadSettingAsync<IList<GameSaveConfig>>(s  => s.GameSaveConfigs);

        return Enum.TryParse(themeName, out ElementTheme cacheTheme) ? cacheTheme : ElementTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await localSettingsService.SaveSettingAsync(s => s.AppBackgroundRequestedTheme, theme.ToString());
    }
}