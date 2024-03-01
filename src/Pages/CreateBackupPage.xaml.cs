using BackMeUp.Data.Management;
using BackMeUp.Models;
using Microsoft.UI.Xaml;

namespace BackMeUp.Pages;

public sealed partial class CreateBackupPage
{
    private readonly Settings _settings; //TODO = SettingsManager.Instance.LoadConfigs();
    private readonly List<string> _gameList;
    public CreateBackupPage()
    {
        this.InitializeComponent();

        _gameList = _settings.GameSaveConfigs.Select(x => x.Game).ToList();
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