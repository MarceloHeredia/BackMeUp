using System.Collections.Generic;
using BackMeUp.Models;

namespace BackMeUp.Contracts;

public interface ISettingsManager
{
    public IReadOnlyList<GameSaveConfig> GameSaveConfigs
    {
        get;
    }
    public string StorageLocation
    {
        get;
    }
    public Settings LoadConfigs();
    public void RestoreDefaultConfigs();
    public bool SaveConfig(GameSaveConfig config);
    public bool RemoveConfig(GameSaveConfig config);
}
