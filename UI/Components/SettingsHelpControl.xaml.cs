using Microsoft.UI.Xaml.Controls;
using System.Reflection;

namespace BackMeUp.UI.Components
{
    public sealed partial class SettingsHelpControl
    {
        private readonly string _appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        public SettingsHelpControl()
        {
            this.InitializeComponent();
        }
    }
}
