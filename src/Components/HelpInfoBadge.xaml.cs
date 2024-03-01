using Microsoft.UI.Xaml.Controls;

namespace BackMeUp.Components
{
    public sealed partial class HelpInfoBadge : UserControl
    {
        public String HelpInfoText { get; set; } = string.Empty;
        public HelpInfoBadge()
        {
            this.InitializeComponent();
        }
    }
}
