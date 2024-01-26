using BackMeUp.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace BackMeUp.UI.Pages
{
    public sealed partial class CreateBackupPage
    {

        private List<string> GameList => ["a", "b"]; //TODO get list of games from database
        public CreateBackupPage()
        {
            this.InitializeComponent();
        }

        private void CreateBackup_Click(object sender, RoutedEventArgs e)
        {
            var saveBackup = new SaveBackup
            {
                GameName = SelectedGameName.SelectedValue.ToString(),
                SaveName = TextSaveName.Text,
                Creation = DateTime.Now,
                Description = TextDescription.Text
            };

            //TODO create backup
        }
    }
}
