using BackMeUp.Data.Models;
using BackMeUp.Data.SettingsManager;
using BackMeUp.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;


namespace BackMeUp.UI.Controls
{

    public enum CreateGameConfigResult
    {
        Created,
        Canceled,
        NoResult
    }

    public sealed partial class CreateGameConfigDialog
    {
        public CreateGameConfigResult Result { get; private set; } = CreateGameConfigResult.NoResult;
        public CreateGameConfigDialog()
        {
            this.InitializeComponent();
        }

        private void ValidateConfigFields()
        {
            if (string.IsNullOrEmpty(GameNameTextBox.Text))
            {
                ErrorInfoBar.Message = ResourceManagementHelper.GetResource("CreateGameConfigDialogGameNameError");
                ErrorInfoBar.IsOpen = true;
                return;
            }

            if (string.IsNullOrEmpty(PickFolderOutputTextBlock.Text))
            {
                ErrorInfoBar.Message = ResourceManagementHelper.GetResource("CreateGameConfigDialogSaveLocationError");
                ErrorInfoBar.IsOpen = true;
            }
        }

        private void GameNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(GameNameTextBox.Text)) return;

            ErrorInfoBar.Message = string.Empty;
            ErrorInfoBar.IsOpen = false;
        }

        private void CreateGameConfigDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ValidateConfigFields();


            args.Cancel = ErrorInfoBar.IsOpen;
            if (ErrorInfoBar.IsOpen) return;

            var deferral = args.GetDeferral();

            SettingsManager.Instance.AddGameSaveConfig(new GameSaveConfig
            {
                Game = GameNameTextBox.Text,
                SavePath = PickFolderOutputTextBlock.Text
            });

            Result = CreateGameConfigResult.Created;
            deferral.Complete();
        }

        private void CreateGameConfigDialog_OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = CreateGameConfigResult.Canceled;
        }

        private async void SaveLocationPicker_OnClick(object sender, RoutedEventArgs e)
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
