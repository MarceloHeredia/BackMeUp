using BackMeUp.Contracts.Services;
using BackMeUp.Pages;
using BackMeUp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Reflection;

namespace BackMeUp.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Func<Page>> _pages = [];
    private readonly Dictionary<string, Type> _pageTypes = [];

    public PageService(IApplicationService applicationService)
    {
        Configure<HomeViewModel, HomePage>(applicationService.GetService<HomePage>);
        //Configure<HomeViewModel>(applicationService.GetService<HomePage>);
        //Configure<BackupsPage>();
        //Configure<CreateBackupPage>();
        Configure<SettingsViewModel, SettingsPage>(applicationService.GetService<SettingsPage>);
    }

    public Page GetPage(string key)
    {
        Func<Page>? pageFactory;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageFactory))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call Configure?");
            }
        }
        return pageFactory();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pageTypes)
        {
            if (!_pageTypes.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }
        return pageType;
    }
    private void Configure<TVm, TV>(Func<Page> pageFactory)
        where TVm : ObservableObject
        where TV : Page
    {
        lock (_pages)
        {
            var key = typeof(TVm).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TV);
            if (_pageTypes.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pageTypes.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, pageFactory);
            _pageTypes.Add(key, type);
        }
    }
}