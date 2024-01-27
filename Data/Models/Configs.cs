using System;
using System.Collections.Generic;

namespace BackMeUp.Data.Models
{
    internal class Configs
    {
        public IList<GameSaveConfig> GameSaveConfigs { get; set; }
        public static Version Version => new(1, 0, 0, 0);
        public DateTime LastUpdated { get; set; }
    }

    internal class GameSaveConfig
    {
        public string Game { get; set; }
        public string SavePath { get; set; }
    }
}
