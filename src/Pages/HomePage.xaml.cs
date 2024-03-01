using BackMeUp.Contracts.Services;
using BackMeUp.ViewModels;

namespace BackMeUp.Pages;

public sealed partial class HomePage
{
    public HomeViewModel ViewModel { get; }
    public HomePage(HomeViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}