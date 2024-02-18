using System;
using System.ComponentModel.DataAnnotations;

namespace BackMeUp.Models;

internal class SaveBackup
{
    [Required]
    public string GameName
    {
        get; init;
    }
    [Required]
    public string SaveName
    {
        get; init;
    }
    public DateTime Creation
    {
        get; init;
    }
    public string Description
    {
        get; init;
    }

    public SaveBackup()
    {
    }
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
        return $"Game: {GameName}\nSave Name: {SaveName}\n\t\t{Creation}\n{Description}";
    }

    public override bool Equals(object obj)
    {
        return obj is SaveBackup saveBackupObj &&
               GameName.Equals(saveBackupObj.GameName) &&
               SaveName.Equals(saveBackupObj.SaveName) &&
               Creation.Equals(saveBackupObj.Creation);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GameName, SaveName, Creation);
    }
}
