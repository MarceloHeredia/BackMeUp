using BackMeUp.Helpers;
using Microsoft.UI.Xaml.Controls;

namespace BackMeUp.Contracts.Services;
public interface INavigationService
{
    event CustomNavigatedEventHandler Navigated;

    bool CanGoBack { get; }
    Frame? Frame { get; set; }
    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);
    bool GoBack();
    void SetListDataItemForNextConnectedAnimation(object item);
}
