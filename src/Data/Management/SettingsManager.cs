﻿using BackMeUp.Contracts;
using BackMeUp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackMeUp.Data.Management;

public sealed class SettingsManager: ISettingsManager
{
    
    private Configs Settings { get; set; }
    public IReadOnlyList<GameSaveConfig> GameSaveConfigs => Settings.GameSaveConfigs.AsReadOnly();

    public string StorageLocation => Settings.StorageLocation;

    public Configs LoadConfigs()
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

    public void RestoreDefaultConfigs()
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