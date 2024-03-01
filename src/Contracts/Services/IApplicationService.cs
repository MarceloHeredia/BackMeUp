using Microsoft.UI.Xaml;

namespace BackMeUp.Contracts.Services;

public interface IApplicationService
{
    T GetService<T>() where T : class;
    nint MainWindowHandle { get; }
    WindowEx MainWindow { get; }
    UIElement? AppTitleBar { get; set; }
}