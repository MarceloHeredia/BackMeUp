using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using SaveMe.Properties;
using SaveMe.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SaveMe
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigureTitleBar();


            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private void ConfigureTitleBar()
        {
            if (!AppWindowTitleBar.IsCustomizationSupported()) return;

            Title = Resources.AppTitle;
    
            AppWindow.SetIcon(Resources.AppIcon);
        }
    }
}
