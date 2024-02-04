using BackMeUp.Data.Models;
using BackMeUp.Data.SettingsManager;
using BackMeUp.UI.Components;
using BackMeUp.Utils;
using BackMeUp.Utils.ExtensionMethods;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BackMeUp.UI.Pages
{
    public sealed partial class SettingsPage
    {
        private readonly SettingsManager _settingsManager = SettingsManager.Instance;
        private readonly ObservableCollection<GameSaveConfigViewItem> _gameSaveConfigs;
        private readonly StandardUICommand _deleteCommand, _editCommand;
        public SettingsPage()
        {
            this.InitializeComponent();

            _deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
            _deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
            _deleteCommand.Description = ResourceManagementHelper.GetResource("SettingsPageDeleteTooltip");

            _editCommand = new StandardUICommand(StandardUICommandKind.Open);
            _editCommand.ExecuteRequested += EditCommand_ExecuteRequested;
            _editCommand.Label = ResourceManagementHelper.GetResource("SettingsPageEdit");
            _editCommand.Description = ResourceManagementHelper.GetResource("SettingsPageEditTooltip");


            _gameSaveConfigs = new ObservableCollection<GameSaveConfigViewItem>(
                _settingsManager.GameSaveConfigs.Select(gameSaveConfig => new GameSaveConfigViewItem()
            {
                GameSaveConfig = gameSaveConfig,
                Delete = _deleteCommand,
                Edit = _editCommand
            }));
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
                    Edit = _editCommand
                }));
        }


        #region Management

        private void EditCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) //TODO implement this
        {
            //if (args.Parameter != null)
            //{
            //    foreach (var i in collection)
            //    {
            //        if (i.Text == (args.Parameter as string))
            //        {
            //            collection.Remove(i);
            //            return;
            //        }
            //    }
            //}
            //if (ListViewRight.SelectedIndex != -1)
            //{
            //    collection.RemoveAt(ListViewRight.SelectedIndex);
            //}
        }

        private void DeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) //TODO implement this
        {
            //if (args.Parameter != null)
            //{
            //    foreach (var i in collection)
            //    {
            //        if (i.Text == (args.Parameter as string))
            //        {
            //            collection.Remove(i);
            //            return;
            //        }
            //    }
            //}
            //if (ListViewRight.SelectedIndex != -1)
            //{
            //    collection.RemoveAt(ListViewRight.SelectedIndex);
            //}
        }

        #endregion
        #region Events

        private void ResetCallback(ResetResult result)
        {
            if (result == ResetResult.Success)
            {
                RefreshListViewItems();
            }
        }
        private async void AddSaveSettings_OnClick(object sender, RoutedEventArgs e)
        {
            CreateGameConfigDialog dialog = new()
            {
                XamlRoot = this.Content.XamlRoot,
            };
            await dialog.ShowAsync();

            if (dialog.Result == CreateGameConfigResult.Created)
            {
                RefreshListViewItems();
            }
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
