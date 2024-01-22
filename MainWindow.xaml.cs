using BackMeUp.Models;
using BackMeUp.Properties;
using BackMeUp.UI.Pages;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

namespace BackMeUp
{
    public sealed partial class MainWindow
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

        private void NavigationControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                return;
                //ContentFrame.Navigate(typeof(SettingsPage));
            }
            var item = args.SelectedItem as NavigationViewItem;
            var tag = (NavigationItemOptions)item!.Tag;

            switch (tag)
            {
                case NavigationItemOptions.Home:
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case NavigationItemOptions.List:
                    //ContentFrame.Navigate(typeof(SavesListViewPage));
                    break;
                case NavigationItemOptions.Create:
                    ContentFrame.Navigate(typeof(CreateBackupPage));
                    break;
            }
        }
    }
}
