using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using BackMeUp.Properties;
using BackMeUp.Services;
using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;

namespace BackMeUp;

public sealed partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Content = null;

        AppWindow.SetIcon(Resources.AppIcon);
    }
}
