using BackMeUp.Contracts.Services;
using BackMeUp.Contracts.ViewModels;
using BackMeUp.Helpers;
using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics.CodeAnalysis;

namespace BackMeUp.Services;


/// <summary>
/// The ContentNavigationService is a simple alternative to the NavigationService that allows the pages to have constructor parameters.
/// It uses the Frame.Content to navigate instead of using the built-in method Frame.Navigate.
/// Since it does not directly navigate, it uses a Stack to keep track of the navigation history.
/// </summary>
public class ContentNavigationService(IApplicationService applicationService, IPageService pageService) : INavigationService
{
    private readonly Stack<string> _navigationStack = new();
    private Frame? _frame;

    public event CustomNavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            return _frame ??= applicationService.MainWindow.Content as Frame;
        }
        set
        {
            _frame = value;
        }
    }
    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]

    public bool CanGoBack => Frame is not null && _navigationStack.Count > 1;

    public bool GoBack()
    {
        if (!CanGoBack)
        {
            return false;
        }

        // Dropping current page
        var currentViewModel = Frame.GetPageViewModel();
        _navigationStack.Pop();

        if (currentViewModel is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }

        // Navigating to the previous page
        var viewModelName = _navigationStack.Peek();

        _frame.Content = pageService.GetPage(viewModelName);
        if (Frame.GetPageViewModel() is INavigationAware toNavigationAware)
        {
            toNavigationAware.OnNavigatedTo(null);
        }
        Navigated?.Invoke(Frame, new(pageService.GetPageType(viewModelName)));

        return true;

    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = pageService.GetPageType(pageKey);

        if (Frame is null || Frame.Content?.GetType() == pageType)
        {
            return false;
        }

        var vmBeforeNavigation = Frame.GetPageViewModel() ?? null;

        var newPage = pageService.GetPage(pageKey);
        _navigationStack.Push(pageKey);

        //Manually navigates by setting Frame.Content
        Frame.Content = newPage;

        if (vmBeforeNavigation is INavigationAware fromNavigationAware)
        {
            fromNavigationAware.OnNavigatedFrom();
        }

        if (Frame.GetPageViewModel() is INavigationAware toNavigationAware)
        {
            toNavigationAware.OnNavigatedTo(parameter);
        }

        Navigated?.Invoke(Frame, new(pageType));

        return true;

    }

    public void SetListDataItemForNextConnectedAnimation(object item) =>
        Frame.SetListDataItemForNextConnectedAnimation(item);

}
