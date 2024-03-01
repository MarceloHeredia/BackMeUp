using BackMeUp.Activation;
using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using BackMeUp.Pages;
using BackMeUp.Services;
using BackMeUp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using WinRT.Interop;

namespace BackMeUp;

public partial class App
{
    public IHost Host { get; }
    private static T GetService<T>() where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }
    private static WindowEx MainWindow { get; } = new MainWindow();

    private static IntPtr MainWindowHandle => WindowNative.GetWindowHandle(MainWindow);

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                // Activation Handlers
                services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                // Services
                services.AddSingleton<IApplicationService>( sp => new ApplicationService(MainWindow, MainWindowHandle, sp));

                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IActivationService, ActivationService>();

                services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();

                services.AddSingleton<IPageService, PageService>();

                //This implementation requires the pages to not have any constructor parameters but gives a better support to GoBack in the navigation
                //services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<INavigationService, ContentNavigationService>();

                services.AddSingleton<INavigationViewService, NavigationViewService>();

                // Views and ViewModels
                services.AddTransient<ShellViewModel>();
                services.AddTransient<ShellPage>();

                services.AddTransient<HomeViewModel>();
                services.AddTransient<HomePage>();

                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsPage>();

                // Configuration

            })
            .Build();

        UnhandledException += App_UnhandledException;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {

        //MainWindow.SetWindowSize(1200, 800);
        //MainWindow.CenterOnScreen();
        //MainWindow.Activate();

        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);

    }

    private static void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        Debug.WriteLine(e.Exception);
    }
}
