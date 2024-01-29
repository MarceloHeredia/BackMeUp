using BackMeUp.Data.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace BackMeUp.Data.ConfigsManagement
{
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
