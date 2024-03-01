using BackMeUp.Contracts;
using BackMeUp.Contracts.Services;
using Microsoft.UI.Xaml;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace BackMeUp.Controls
{
    public sealed partial class SettingsGeneralControl
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IApplicationService _applicationService;
        public SettingsGeneralControl()
        {
            this.InitializeComponent();
        }
        private async void BackupLocationPicker_OnClick(object sender, RoutedEventArgs e)
        {
            PickFolderOutputTextBlock.Text = "";
            var folderPicker = new FolderPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            folderPicker.FileTypeFilter.Add("*");
            var hWnd = _applicationService.MainWindowHandle;

            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);
            var folder = await folderPicker.PickSingleFolderAsync();

            if (folder is null) return;

            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PickFolderOutputTextBlock.Text = folder.Path;
        }

        private void RestoreDefaultBackupLocation_OnClick(object sender, RoutedEventArgs e)
        {
            var location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BackMeUp");

            if (string.IsNullOrEmpty(location)) return;

            try
            {
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }

                File.WriteAllText(Path.Combine(location, "test.txt"), "Some Random Things");
            }
            catch{}
        }
    }
}
