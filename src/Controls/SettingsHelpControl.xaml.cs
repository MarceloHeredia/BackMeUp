using System.Reflection;

namespace BackMeUp.Controls
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
