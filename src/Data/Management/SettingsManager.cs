using BackMeUp.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackMeUp.Data.Management;

internal sealed class SettingsManager
{
    
    private Configs Settings { get; set; }
    internal IReadOnlyList<GameSaveConfig> GameSaveConfigs => Settings.GameSaveConfigs.AsReadOnly();

    internal string StorageLocation => Settings.StorageLocation;

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
    {//TODO: move files?
        if (Directory.Exists(DefaultConfigs.ConfigsFolder) &&
            File.Exists(DefaultConfigs.ConfigsFile))
        {
            File.Delete(DefaultConfigs.ConfigsFile);
        }

        Settings = DefaultConfigs.DefaultConfigsData;
        Write();
    }

    private bool Write()
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

    #region Constructor
    private static readonly Lazy<SettingsManager> Lazy = new(() => new SettingsManager());
    public static SettingsManager Instance => Lazy.Value;
    private SettingsManager()
    {
        Settings = LoadConfigs();
    }
    #endregion
}