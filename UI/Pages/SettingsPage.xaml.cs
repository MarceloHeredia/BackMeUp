using BackMeUp.Data.ConfigsManagement;
using BackMeUp.Data.Models;
using Microsoft.UI.Xaml.Controls;

namespace BackMeUp.UI.Pages
{
    public sealed partial class SettingsPage
    {
        private readonly Configs _configs = ConfigsManagement.LoadConfigs();
        public SettingsPage()
        {
            this.InitializeComponent();
        }
    }
}
