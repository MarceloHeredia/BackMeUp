using System;
using BackMeUp.Data.Models;

namespace BackMeUp.Utils.Behaviors
{
    internal static class SaveBackupBehavior
    {
        internal static bool FilterContains(this SaveBackup sb, string fGameName, string fSaveName)
        {
            return sb.GameName.Contains(fGameName, StringComparison.InvariantCultureIgnoreCase) &&
                   sb.SaveName.Contains(fSaveName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
