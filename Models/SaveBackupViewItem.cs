﻿using System.Windows.Input;

namespace BackMeUp.Models
{
    internal class SaveBackupViewItem
    {
        public SaveBackup SaveBackup { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Restore { get; set; }
    }
}
