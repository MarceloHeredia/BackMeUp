using BackMeUp.Activation;
using BackMeUp.Contracts;
using BackMeUp.Contracts.Services;
using BackMeUp.Services;
using BackMeUp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using WinRT.Interop;

namespace BackMeUp;

public partial class App: Application, IAppContext
{
    public IHost Host { get; private set; }
    public static T GetService<T>()
        where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public IntPtr MainWindowHandle => WindowNative.GetWindowHandle(MainWindow);

    public static WindowEx MainWindow { get; } = new MainWindow();
    public static UIElement? AppTitlebar { get; set; }
    public App()
    {
        InitializeComponent();

        ConfigureHost();

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

    private void ConfigureHost()
    {
        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                // Activation Handlers
                services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                // Services


                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Views and ViewModels
                services.AddTransient<HomeViewModel>();
                services.AddTransient<MainPage>();

                // Configuration

            })
            .Build();
    }

    private static void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        Debug.WriteLine(e.Exception);
    }
}
