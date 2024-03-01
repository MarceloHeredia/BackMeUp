using BackMeUp.Contracts.Services;
using BackMeUp.Contracts.ViewModels;
using BackMeUp.Helpers;
using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace BackMeUp.Services;

// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
/// <summary>
/// This NavigationService is based on the Template Studio. It requires the pages to not have any constructor parameters but gives a better support to GoBack in the navigation.
/// </summary>
/// <param name="applicationService">ApplicationService gives the context of the MainWindow</param>
/// <param name="pageService">PageService is used to navigate from Page to Page</param>
public class NavigationService(IApplicationService applicationService, IPageService pageService) : INavigationService
{
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event CustomNavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (_frame is null)
            {
                _frame = applicationService.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }
        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }
    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]

    public bool CanGoBack => Frame is not null && Frame.CanGoBack;

    public bool GoBack()
    {
        if (CanGoBack)
        {
            var vmBeforeNavigation = _frame.GetPageViewModel();
            _frame.GoBack();
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = pageService.GetPageType(pageKey);

        if (_frame is not null && (_frame.Content?.GetType() != pageType ||
                                   (parameter is not null && !parameter.Equals(_lastParameterUsed))))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();

            var navigated = _frame.Navigate(pageType, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    public void SetListDataItemForNextConnectedAnimation(object item) =>
        Frame.SetListDataItemForNextConnectedAnimation(item);

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            Navigated?.Invoke(sender, new(e.SourcePageType));
        }
    }

    private void RegisterFrameEvents()
    {
        if (_frame is not null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame is not null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }
}