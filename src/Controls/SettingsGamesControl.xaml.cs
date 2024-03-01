using BackMeUp.Data.Management;
using BackMeUp.Helpers;
using BackMeUp.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;


namespace BackMeUp.Controls
{
    public sealed partial class SettingsGamesControl
    {
        private readonly SettingsManager _settingsManager; // =  TODO SettingsManager.Instance;
        private readonly ObservableCollection<GameSaveConfigViewItem> _gameSaveConfigs;
        private readonly StandardUICommand _deleteCommand;
        //TODO: Investigate if Edit button is needed
        public SettingsGamesControl()
        {
            this.InitializeComponent();

            _deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
            _deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
            _deleteCommand.Description = ResourcesManager.GetResource("SettingsPageDeleteTooltip");


            _gameSaveConfigs = new ObservableCollection<GameSaveConfigViewItem>(
                _settingsManager.GameSaveConfigs.Select(gameSaveConfig => new GameSaveConfigViewItem()
                {
                    GameSaveConfig = gameSaveConfig,
                    Delete = _deleteCommand,
                }));
        }


        private void DeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            if (args.Parameter is not GameSaveConfig gameSaveConfig) return;
            _settingsManager.RemoveConfig(gameSaveConfig);

            _gameSaveConfigs.RemoveAll(gsConfigViewItem => gsConfigViewItem.GameSaveConfig.Equals(gameSaveConfig));

        }

        private void RefreshListViewItems()
        {
            _gameSaveConfigs.RemoveAll(gsConfigViewItem => !_settingsManager.GameSaveConfigs.Contains(gsConfigViewItem.GameSaveConfig));
            _gameSaveConfigs.AddRange(_settingsManager.GameSaveConfigs
                .Where(gsConfig => _gameSaveConfigs.All(gsConfigViewItem => !gsConfigViewItem.GameSaveConfig.Equals(gsConfig)))
                .Select(gsConfig => new GameSaveConfigViewItem()
                {
                    GameSaveConfig = gsConfig,
                    Delete = _deleteCommand,
                }));
        }


        #region Events

        private void ResetCallback(ResetResult result)
        {
            if (result == ResetResult.Success)
            {
                RefreshListViewItems();
            }
        }
        private async void AddSaveSettings_OnClick(object sender, RoutedEventArgs e)
        {//TODO
            //CreateGameConfigDialog dialog = new()
            //{
            //    XamlRoot = this.Content.XamlRoot,
            //};
            //await dialog.ShowAsync();

            //if (dialog.Result == CreateGameConfigResult.Created)
            //{
            //    RefreshListViewItems();
            //}
        }

        #endregion
        #region Hover Animations
        private void SettingsSwipe_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType is (Microsoft.UI.Input.PointerDeviceType)Windows.Devices.Input.PointerDeviceType.Mouse or (Microsoft.UI.Input.PointerDeviceType)Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }

        private void SettingsSwipe_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
        }
        #endregion
    }
}
