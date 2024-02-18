using BackMeUp.Contracts.Services;
using BackMeUp.ViewModels;
using Microsoft.UI.Xaml;

namespace BackMeUp.Activation;

public class DefaultActivationHandler(INavigationService navigationService)
    : ActivationHandler<LaunchActivatedEventArgs>
{
    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers can handle the activation.
        return navigationService.Frame?.Content is null;
    }

    protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        navigationService.NavigateTo(typeof(HomeViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
