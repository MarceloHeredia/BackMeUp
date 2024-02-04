using System;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using WinUIEx;

namespace BackMeUp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public static MainWindow MainWindow { get; private set; }
        public static IntPtr MainWindowHandle => WindowNative.GetWindowHandle(MainWindow);

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();

            MainWindow.SetWindowSize(1200, 800);
            MainWindow.CenterOnScreen();

            MainWindow.Activate();

        }

    }
}
