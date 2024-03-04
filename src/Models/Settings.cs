﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BackMeUp.Models
{
    public class Settings
    {
        public required string AppBackgroundRequestedTheme { get; set; }
        public required string StorageLocation { get; set; }
        public required IList<GameSaveConfig> GameSaveConfigs { get; set; }
        [JsonProperty(nameof(Version))]
        public static Version ConfigVersion => new(1, 0, 0);
        public DateTime LastUpdated { get; set; }
    }

    public class GameSaveConfig
    {
        public required string Game { get; init; }
        public required string SavePath { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not GameSaveConfig gsConfigObj) return false;

            return Game.Equals(gsConfigObj.Game) && SavePath.Equals(gsConfigObj.SavePath);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Game, SavePath);
        }
    }
}
