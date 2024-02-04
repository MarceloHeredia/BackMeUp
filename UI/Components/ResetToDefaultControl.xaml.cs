using BackMeUp.Data.SettingsManager;
using Microsoft.UI.Xaml;


namespace BackMeUp.UI.Components
{
    public delegate void ResetEventHandler(ResetResult result);
    public enum ResetResult
    {
        Success,
        Failure // Not using this for now at least.
    }

    public sealed partial class ResetToDefaultControl
    {

        public event ResetEventHandler ResetButtonClick;
        public ResetToDefaultControl()
        {
            this.InitializeComponent();
        }


        private void ResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.Instance.RestoreDefaultConfigs();
            ResetButtonClick?.Invoke(ResetResult.Success);
            ResetToDefaultFlyout.Hide();
            ResetToDefaultFlyout2.Hide();
        }
        private void ResetToDefaultFlyout2_OnClosed(object sender, object e)
        {
            ResetToDefaultFlyout.Hide();
        }
    }
}
