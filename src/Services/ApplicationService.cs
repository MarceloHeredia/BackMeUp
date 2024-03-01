using BackMeUp.Contracts.Services;
using Microsoft.UI.Xaml;

namespace BackMeUp.Services;
public class ApplicationService(
    WindowEx mainWindow,
    IntPtr mainWindowHandle,
    IServiceProvider serviceProvider)
    : IApplicationService
{
    public WindowEx MainWindow => mainWindow;
    public IntPtr MainWindowHandle => mainWindowHandle;
    public UIElement? AppTitleBar { get; set; }

    public T GetService<T>() where T : class
    {
        if (serviceProvider.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }
}
