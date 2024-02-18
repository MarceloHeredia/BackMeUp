using System;
using BackMeUp.Models;

namespace BackMeUp.Helpers;

internal static class SaveBackupExtensions
{
    internal static bool FilterContains(this SaveBackup sb, string fGameName, string fSaveName)
    {
        return sb.GameName.Contains(fGameName, StringComparison.InvariantCultureIgnoreCase) &&
               sb.SaveName.Contains(fSaveName, StringComparison.InvariantCultureIgnoreCase);
    }
}
