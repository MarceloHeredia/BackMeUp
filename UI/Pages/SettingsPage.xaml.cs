using System.Collections.ObjectModel;
using System.Linq;
using BackMeUp.Data.ConfigsManagement;
using BackMeUp.Data.Models;
using BackMeUp.Utils;
using BackMeUp.Utils.ExtensionMethods;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BackMeUp.UI.Pages
{
    public sealed partial class SettingsPage
    {
        private readonly Configs _configs = ConfigsManagement.LoadConfigs();
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

            for (var i = 0; i < 20; i++)
            {

                _configs.GameSaveConfigs.Add(new GameSaveConfig
                {
                    Game = "Some Game",
                    SavePath = "C:\\SomeGame\\Save",
                });
            }

            _gameSaveConfigs = new ObservableCollection<GameSaveConfigViewItem>(_configs.GameSaveConfigs.Select(gameSaveConfig => new GameSaveConfigViewItem()
            {
                GameSaveConfig = gameSaveConfig,
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
        private void ResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            ConfigsManagement.RestoreDefaultConfigs();
            _gameSaveConfigs.Clear();
            _gameSaveConfigs.AddRange(ConfigsManagement
                .LoadConfigs().GameSaveConfigs.Select(gameSaveConfig => new GameSaveConfigViewItem()
                    {
                    GameSaveConfig = gameSaveConfig,
                    Delete = _deleteCommand,
                    Edit = _editCommand
                }));
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
