using BackMeUp.Utils;
using BackMeUp.Utils.Behaviors;
using BackMeUp.Utils.ExtensionMethods;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BackMeUp.Data.Models;


namespace BackMeUp.UI.Pages
{
    public sealed partial class BackupsPage
    {
        private readonly List<SaveBackup> _saves = []; //TODO get list of saves from database
        private readonly ObservableCollection<SaveBackupViewItem> _allSaves = [];
        private readonly ObservableCollection<SaveBackupViewItem> _filteredSaves;
        private readonly StandardUICommand _deleteCommand, _restoreCommand;

        public BackupsPage()
        {
            InitializeComponent();

            _deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
            _deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
            _deleteCommand.Description = ResourceManagementHelper.GetResource("BackupsPageDeleteTooltip");

            _restoreCommand = new StandardUICommand(StandardUICommandKind.Copy);
            _restoreCommand.ExecuteRequested += RestoreCommand_ExecuteRequested;
            _restoreCommand.Label = ResourceManagementHelper.GetResource("BackupsPageRestore");
            _restoreCommand.Description = ResourceManagementHelper.GetResource("BackupsPageRestoreTooltip");


            SomeTest().ForEach(saveBackup =>
            {
                _saves.Add(saveBackup);
                _allSaves.Add(new SaveBackupViewItem { SaveBackup = saveBackup, Delete = _deleteCommand, Restore = _restoreCommand });
            });
            _filteredSaves = new ObservableCollection<SaveBackupViewItem>(_allSaves.OrderByDescending(sb => sb.SaveBackup.Creation));
        }


        private static List<SaveBackup> SomeTest()
        {
            return
                [
                    new SaveBackup("Sekiro", "Sekirao", DateTime.Today),
                    new SaveBackup("Sekiro", "Sekirin", DateTime.Today.AddDays(-1)),
                    new SaveBackup("Sekiro", "Sekira12312o", DateTime.Today.AddDays(1)),
                    new SaveBackup("Sekiro", "Sekiraxco"),
                    new SaveBackup("Sekiro", "Sekirasdao"),
                    new SaveBackup("Sekiro", "Sekirao"),
                    new SaveBackup("Sekiro", "Sekirazasdo"),
                    new SaveBackup("Dark Souls", "Sekirazasdo"),
                ];
        }
        #region Filters


        private void Remove_NonMatching(IEnumerable<SaveBackupViewItem> filteredData)
        {
            _filteredSaves.RemoveAll(fs => !filteredData.Contains(fs));
        }

        private void AddBack_SaveViewItems(IEnumerable<SaveBackupViewItem> filteredData)
            // When a user hits backspace, more items may need to be added back into the list
        {
            var notPresentFilteredData = filteredData.Where(fd => !_filteredSaves.Contains(fd));

            foreach (var fd in notPresentFilteredData)
            {
                var didInsert = false;
                for (var i = 0; i < _filteredSaves.Count; i++)
                {
                    if (_filteredSaves[i].SaveBackup.Creation > fd.SaveBackup.Creation) continue;

                    _filteredSaves.Insert(i, fd);
                    didInsert= true;
                    break;
                }

                if (!didInsert)
                {
                    _filteredSaves.Add(fd);
                }
            }
        }

        private void OnFilterChanged(object sender, TextChangedEventArgs args)
        {
            var filtered = _allSaves
                .Where(sd => sd.SaveBackup.FilterContains(GameNameFilter.Text, SaveNameFilter.Text))
                .OrderByDescending(sb => sb.SaveBackup.Creation);

            Remove_NonMatching(filtered);
            AddBack_SaveViewItems(filtered);
        }
        #endregion

        #region List Management

        private void RestoreCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args) //TODO implement this
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

        private void SavesListSwipe_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType is (Microsoft.UI.Input.PointerDeviceType)Windows.Devices.Input.PointerDeviceType.Mouse or (Microsoft.UI.Input.PointerDeviceType)Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }

        private void SavesListSwipe_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
        }
        #endregion
    }
}
