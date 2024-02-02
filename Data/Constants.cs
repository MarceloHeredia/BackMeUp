
using BackMeUp.Data.Models;
using BackMeUp.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackMeUp.Data
{
    internal static class DefaultConfigs
    {
        internal static string ConfigsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Resources.AppFolder);
        internal static string ConfigsFile => Path.Combine(ConfigsFolder, Resources.ConfigsFile);

        internal static Configs DefaultConfigsData => new()
        {
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
