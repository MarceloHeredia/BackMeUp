using BackMeUp.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackMeUp.Data.Management
{
    internal static class BackupStorageManager
    {
        internal static class ManageBackupStorage
        {
            private static void ValidateSaveBackup(SaveBackup saveBackup)
            {
                var validationContext = new ValidationContext(saveBackup, null, null);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(saveBackup, validationContext, validationResults, true);

                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                    {
                        Debug.WriteLine($"Field '{validationResult.MemberNames.First()}' is required");
                    }
                }
            }

            /// <summary>
            /// Create backup from the actual game save
            /// </summary>
            /// <param name="saveBackup"></param>
            /// <exception cref="ArgumentNullException"></exception>
            internal static bool CreateGameBackup(SaveBackup saveBackup)
            {

                ValidateSaveBackup(saveBackup);
                var gameSaveConfig = SettingsManager.Instance.GameSaveConfigs
                    .FirstOrDefault(gsc => gsc.Game.Equals(saveBackup.GameName));

                try
                {

                    if (gameSaveConfig?.SavePath is not null)
                    {
                        var saveLocation = Path.Combine(SettingsManager.Instance.StorageLocation, $@"{saveBackup.SaveName}.zip");
                        ZipFile.CreateFromDirectory(gameSaveConfig.SavePath, saveLocation, CompressionLevel.Fastest, false);

                        if (File.Exists(saveLocation)) return true;

                        return false;
                    }
                    throw new ArgumentException("GameName", "Invalid Game Name");

                }
                catch (ArgumentException argEx)
                {
                    Debug.WriteLine(argEx.Message);
                    return false;
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    Debug.WriteLine(dirEx.Message);
                    return false;
                }
                catch (IOException ioEx)
                {
                    Debug.WriteLine(ioEx.Message);
                    return false;
                }
            }

            /// <summary>
            /// Restores a backup to the game save folder
            /// </summary>
            /// <param name="saveBackup"></param>
            /// <exception cref="ArgumentNullException"></exception>
            internal static void RestoreGameBackup(SaveBackup saveBackup)
            {

                if (saveBackup is null || saveBackup?.GameName is null)
                {
                    throw new ArgumentNullException("SaveBackup", "Save Backup Object should not be null");
                }

                Configs.GamesSavePath().TryGetValue(saveBackup.GameName, out var gamePath);

                try
                {
                    if (gamePath is not null)
                    {
                        var gamePathDir = new DirectoryInfo(gamePath);
                        gamePathDir.Empty();

                        ZipFile.ExtractToDirectory(Path.Combine(Configs.BackupPath, $@"{saveBackup.SaveName}.zip"), gamePath);
                    }
                    throw new ArgumentException("GameName", "Invalid Game Name");
                }
                catch (ArgumentException argEx)
                {
                    Debug.WriteLine(argEx.Message);
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    Debug.WriteLine(dirEx.Message);
                }
                catch (IOException ioEx)
                {
                    Debug.WriteLine(ioEx.Message);
                }
            }

            /// <summary>
            /// Deletes the backup zip file from the Manager storage
            /// </summary>
            /// <param name="saveBackup"></param>
            /// <exception cref="ArgumentNullException"></exception>
            internal static void DeleteGameBackup(SaveBackup saveBackup)
            {

                if (saveBackup is null || saveBackup?.GameName is null)
                {
                    throw new ArgumentNullException("SaveBackup", "Save Backup Object should not be null");
                }

                try
                {
                    File.Delete(Path.Combine(Configs.BackupPath, $@"{saveBackup.SaveName}.zip"));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
        }
}
