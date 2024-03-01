using BackMeUp.Contracts;
using BackMeUp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackMeUp.Data.Management;

public sealed class SettingsManager: ISettingsManager
{
    
    private Settings Settings { get; set; }
    public IReadOnlyList<GameSaveConfig> GameSaveConfigs => Settings.GameSaveConfigs.AsReadOnly();

    public string StorageLocation => Settings.StorageLocation;

    public Settings LoadConfigs()
    {
        if (!Directory.Exists(DefaultSettings.ConfigsFolder))
        {
            Directory.CreateDirectory(DefaultSettings.ConfigsFolder);
        }

        if (!File.Exists(DefaultSettings.ConfigsFile))
        {
            // TODO: Log error or something before restoring
            RestoreDefaultConfigs();
            return DefaultSettings.DefaultSettingsData;
        }

        var json = File.ReadAllText(DefaultSettings.ConfigsFile);
        var configs = JsonConvert.DeserializeObject<Settings>(json);
        return configs;

    }

    public void RestoreDefaultConfigs()
    {//TODO: move files?
        if (Directory.Exists(DefaultSettings.ConfigsFolder) &&
            File.Exists(DefaultSettings.ConfigsFile))
        {
            File.Delete(DefaultSettings.ConfigsFile);
        }

        Settings = DefaultSettings.DefaultSettingsData;
        Write();
    }

    private bool Write()
    {
        try
        {
            var jsonConfigs = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(DefaultSettings.ConfigsFile, jsonConfigs);
            return true;
        }
        catch (Exception) // TODO: Log error
        {
            return false;
        }
    }

    public bool SaveConfig(GameSaveConfig gameSaveConfig)
    {
        Settings.GameSaveConfigs.Add(gameSaveConfig);
        return Write();
    }

    public bool RemoveConfig(GameSaveConfig gameSaveConfig)
    {
        Settings.GameSaveConfigs.Remove(gameSaveConfig);
        return Write();
    }

    #region Constructor
    public SettingsManager()
    {
        Settings = LoadConfigs();
    }
    #endregion
}