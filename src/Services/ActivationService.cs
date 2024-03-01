using BackMeUp.Activation;
using BackMeUp.Contracts.Services;
using BackMeUp.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BackMeUp.Services
{
    public class ActivationService(
        IApplicationService applicationService,
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor -> This change breaks the code completely don't do it
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IThemeSelectorService themeSelectorService)
        : IActivationService
    {
        public async Task ActivateAsync(object activationArgs)
        {
            // Execute tasks before activation.
            await InitializeAsync();

            // Set the MainWindow Content.
            if (applicationService.MainWindow.Content is null)
            {
                UIElement? shell = applicationService.GetService<ShellPage>();
                applicationService.MainWindow.Content = shell ?? new Frame();
            }

            await HandleActivationAsync(activationArgs);

            applicationService.MainWindow.Activate();

            await StartupAsync();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler is not null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }
            else if (defaultHandler.CanHandle(activationArgs))
            {
                await defaultHandler.HandleAsync(activationArgs);
            }
        }

        private async Task InitializeAsync()
        {
            await themeSelectorService.InitializeAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await themeSelectorService.SetRequestedThemeAsync();
            await Task.CompletedTask;
        }
    }
}
