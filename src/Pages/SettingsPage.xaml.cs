using BackMeUp.ViewModels;

namespace BackMeUp.Pages;

public sealed partial class SettingsPage
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}