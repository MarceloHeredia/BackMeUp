using BackMeUp.Data.Management;
using BackMeUp.Data.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackMeUp.UI.Pages
{
    public sealed partial class CreateBackupPage
    {
        private readonly Configs _configs = SettingsManager.Instance.LoadConfigs();
        private readonly List<string> _gameList;
        public CreateBackupPage()
        {
            this.InitializeComponent();

            _gameList = _configs.GameSaveConfigs.Select(x => x.Game).ToList();
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
