using BackMeUp.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackMeUp.Data.SettingsManager;

internal sealed class SettingsManager
{
    private static readonly Lazy<SettingsManager> Lazy = new(() => new SettingsManager());

    public static SettingsManager Instance => Lazy.Value;

    private SettingsManager()
    {
        Settings = LoadConfigs();
    }

    internal Configs Settings { get; private set; }
    internal IList<GameSaveConfig> GameSaveConfigs => Settings.GameSaveConfigs;

    internal Configs LoadConfigs()
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

    internal void RestoreDefaultConfigs()
    {
        if (Directory.Exists(DefaultConfigs.ConfigsFolder) &&
            File.Exists(DefaultConfigs.ConfigsFile))
        {
            File.Delete(DefaultConfigs.ConfigsFile);
        }

        Settings = DefaultConfigs.DefaultConfigsData;
        Write();
    }

    internal bool Write()
    {
        try
        {
            var jsonConfigs = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(DefaultConfigs.ConfigsFile, jsonConfigs);
            return true;
        }
        catch (Exception) // TODO: Log error
        {
            return false;
        }
    }

    internal bool AddGameSaveConfig(GameSaveConfig gameSaveConfig)
    {
        Settings.GameSaveConfigs.Add(gameSaveConfig);
        return Write();
    }

    internal bool RemoveGameSaveConfig(GameSaveConfig gameSaveConfig)
    {
        Settings.GameSaveConfigs.Remove(gameSaveConfig);
        return Write();
    }
}