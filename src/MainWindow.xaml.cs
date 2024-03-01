using BackMeUp.Properties;
using Windows.UI.ViewManagement;

namespace BackMeUp;

public sealed partial class MainWindow
{
    private readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue;
    private readonly UISettings _settings;

    public MainWindow()
    {
        InitializeComponent();
        Content = null;

        AppWindow.SetIcon(Resources.AppIcon);

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _settings = new();
        _settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    // this handles updating the caption button colors correctly when Windows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args) //TODO:
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        //_dispatcherQueue.TryEnqueue(() =>
        //{
        //    TitleBarHelper.ApplySystemThemeToCaptionButtons();
        //});
    }
}
