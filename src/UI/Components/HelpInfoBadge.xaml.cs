using Microsoft.UI.Xaml.Controls;
using System;

namespace BackMeUp.UI.Components
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
