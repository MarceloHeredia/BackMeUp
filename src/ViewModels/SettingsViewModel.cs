using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace BackMeUp.ViewModels
{
    public partial class SettingsViewModel : ObservableRecipient
    {
        [ObservableProperty] private ElementTheme _elementTheme;
        [ObservableProperty] private string _versionDescription;

        public ICommand SwitchThemeCommand { get; }

        public SettingsViewModel(IThemeSelectorService themeSelectorService)
        {
            _elementTheme = themeSelectorService.Theme;
            _versionDescription = GetVersionDescription();

            SwitchThemeCommand = new RelayCommand<ElementTheme>(
                 (param) =>
                {
                    if (ElementTheme != param)
                    {
                        ElementTheme = param; 
                        themeSelectorService.SetThemeAsync(param);
                    }
                });
        }

        private static string GetVersionDescription()
        {
            Version version;

            if (RuntimeHelper.IsMsix)
            {
                var packageVersion = Package.Current.Id.Version;

                version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
            }
            else
            {
                version = Assembly.GetExecutingAssembly().GetName().Version!;
            }

            return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
