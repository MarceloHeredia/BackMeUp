using BackMeUp.Models;
using BackMeUp.Properties;

namespace BackMeUp.Data
{
    internal static class DefaultSettings
    {
        internal static string ConfigsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Resources.AppFolder);
        internal static string ConfigsFile => Path.Combine(ConfigsFolder, Resources.LocalSettingsFile);
        internal static string BackupFolder => Path.Combine(ConfigsFolder, Resources.BackupFolder);

        internal static Settings DefaultSettingsData => new()
        {
            StorageLocation = BackupFolder,
            GameSaveConfigs = KnownSaveGameLocations,
            LastUpdated = DateTime.Now,
        };

        internal static IList<GameSaveConfig> KnownSaveGameLocations => new List<GameSaveConfig>
        {
            new()
            {
                Game = Resources.SekiroName,
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sekiro")
            },
            new()
            {
                Game = Resources.DSRName,
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"FromSoftware\DARK SOULS REMASTERED")
            },
            new()
            {
                Game = Resources.DS2Name,
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarkSoulsII")
            },
            new()
            {
                Game = Resources.DS3Name,
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarkSoulsIII")
            }
        };
    }
}
