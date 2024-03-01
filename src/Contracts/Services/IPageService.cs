using Microsoft.UI.Xaml.Controls;

namespace BackMeUp.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
    Page GetPage(string key);
}