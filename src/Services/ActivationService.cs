using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using BackMeUp.Activation;
using BackMeUp.Contracts.Services;
using BackMeUp.Pages;

namespace BackMeUp.Services
{
    public class ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler,
        IEnumerable<IActivationHandler> activationHandlers,
        IThemeSelectorService themeSelectorService)
        : IActivationService
    {
        private UIElement? _shell = null;

        public async Task ActivateAsync(object activationArgs)
        {
            // Execute tasks before activation.
            await InitializeAsync();

            // Set the MainWindow Content.
            if (App.MainWindow.Content is null)
            {
                _shell = App.GetService<ShellPage>();
                App.MainWindow.Content = _shell ?? new Frame();
            }

            await HandleActivationAsync(activationArgs);

            App.MainWindow.Activate();

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
