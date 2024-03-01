using BackMeUp.Behaviors;
using BackMeUp.Contracts.Services;
using BackMeUp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Windows.System;


namespace BackMeUp.Pages;

public sealed partial class ShellPage
{
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
    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        //TitleBarHelper.UpdateTitleBar(RequestedTheme);

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
