using BackMeUp.Behaviors;
using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using BackMeUp.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;

namespace BackMeUp.ViewModels;

/// <summary>
/// ShellViewModel is the ViewModel for the ShellPage. It is responsible for the navigation of the UI.
/// </summary>
public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty] private bool _isBackEnabled;
    [ObservableProperty] private object? _selected;

    public INavigationService NavigationService { get; }
    public INavigationViewService NavigationViewService { get; }
    public NavigationViewHeaderBehavior NavigationViewHeaderBehavior { get; }


    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, IApplicationService applicationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += NavigationService_Navigated;
        NavigationViewService = navigationViewService;

        NavigationViewHeaderBehavior = new(applicationService);
    }

    private void NavigationService_Navigated(object sender, CustomNavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }
        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem is not null)
        {
            Selected = selectedItem;
        }
    }
}
