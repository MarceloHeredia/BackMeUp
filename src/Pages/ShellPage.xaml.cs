using BackMeUp.Behaviors;
using BackMeUp.Contracts.Services;
using BackMeUp.Helpers;
using BackMeUp.Services;
using BackMeUp.ViewModels;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using Windows.UI.ViewManagement;


namespace BackMeUp.Pages;

public sealed partial class ShellPage
{
    private readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue;
    private readonly UISettings _settings;

    public ShellViewModel ViewModel { get; }
    public IApplicationService ApplicationService { get; }

    public ShellPage(ShellViewModel viewModel, IApplicationService applicationService)
    {
        ViewModel = viewModel;
        ApplicationService = applicationService;

        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        var binding = new Binding
        {
            Source = ViewModel, Path = new PropertyPath("Selected.Content"), Mode = BindingMode.OneWay
        };

        BindingOperations.SetBinding(ViewModel.NavigationViewHeaderBehavior, NavigationViewHeaderBehavior.DefaultHeaderProperty, binding);

        //ViewModel.NavigationViewHeaderBehavior.DefaultHeader = (ViewModel.Selected as ContentControl)?.Content;
        ViewModel.NavigationViewHeaderBehavior.DefaultHeaderTemplate = (DataTemplate)Resources["DefaultHeaderTemplate"];


        ApplicationService.MainWindow.ExtendsContentIntoTitleBar = true;
        ApplicationService.MainWindow.SetTitleBar(AppTitleBar);
        ApplicationService.MainWindow.Activated += MainWindow_Activated;


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
        _dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons(ApplicationService.AppTitleBar as FrameworkElement, (ApplicationService.MainWindow as MainWindow)!);
        });
    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme, (ApplicationService.MainWindow as MainWindow)!);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }
    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        ApplicationService.AppTitleBar = AppTitleBarText;
    }
    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }
    private KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = ApplicationService.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
}
