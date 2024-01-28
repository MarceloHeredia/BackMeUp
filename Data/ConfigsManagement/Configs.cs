using BackMeUp.Data.Models;
using BackMeUp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackMeUp.Data.ConfigsManagement
{
    internal static class DefaultConfigs
    {
        internal static string ConfigsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BackMeUp");
        internal static string ConfigsFile => Path.Combine(ConfigsFolder, "configs.json");

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

    internal static class ConfigsManagement
    {
        internal static Configs LoadConfigs()
        {
            if (!Directory.Exists(DefaultConfigs.ConfigsFolder))
            {
                Directory.CreateDirectory(DefaultConfigs.ConfigsFolder);
            }

            if (!File.Exists(DefaultConfigs.ConfigsFile))
            {
                // TODO: Log error or something before restoring
                RestoreDefaultConfigs();
                return DefaultConfigs.DefaultConfigsData;
            }

            var json = File.ReadAllText(DefaultConfigs.ConfigsFile);
            var configs = JsonConvert.DeserializeObject<Configs>(json);
            return configs;

        }

        internal static void RestoreDefaultConfigs()
        {
            if (Directory.Exists(DefaultConfigs.ConfigsFolder) &&
                File.Exists(DefaultConfigs.ConfigsFile))
            {
                File.Delete(DefaultConfigs.ConfigsFile);
            }

            WriteConfigs(DefaultConfigs.DefaultConfigsData);
        }

        internal static bool WriteConfigs(Configs configs)
        {
            try
            {
                var jsonConfigs = JsonConvert.SerializeObject(configs, Formatting.Indented);
                File.WriteAllText(DefaultConfigs.ConfigsFile, jsonConfigs);
                return true;
            }
            catch (Exception) // TODO: Log error
            {
                return false;
            }
        }
    }
}
