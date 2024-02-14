using BackMeUp.Data.SettingsManager;
using Microsoft.UI.Xaml;
using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace BackMeUp.UI.Controls
{
    public sealed partial class SettingsGeneralControl
    {
        private SettingsManager _settingsManager = SettingsManager.Instance;
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
            var hWnd = App.MainWindowHandle;

            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);
            var folder = await folderPicker.PickSingleFolderAsync();

            if (folder is null) return;

            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PickFolderOutputTextBlock.Text = folder.Path;
        }
    }
}
