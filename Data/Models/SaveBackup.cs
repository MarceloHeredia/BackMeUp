using System;

namespace BackMeUp.Data.Models
{
    internal class SaveBackup
    {
        public string GameName { get; set; }
        public string SaveName { get; set; }
        public DateTime Creation { get; set; }
        public string Description { get; set; }

        public SaveBackup() { }
        public SaveBackup(string gameName, string saveName, DateTime creation = new(), string description = "")
        {
            GameName = gameName;
            SaveName = saveName;
            Creation = creation;
            Description = description;
        }
        //public static SaveBackup FromDataReader(IDataReader reader)
        //{
        //    return new SaveBackup()
        //    {
        //        GameName = reader[Resources.GameNameField].ToString(),
        //        SaveName = reader[Resources.SaveNameField].ToString(),
        //        Creation = Convert.ToDateTime(reader[Resources.CreationField]),
        //    };
        //}

        public override string ToString()
        {
            return $"Game: {this.GameName}\nSave Name: {this.SaveName}\n\t\t{this.Creation}\n{this.Description}";
        }
    }
}
