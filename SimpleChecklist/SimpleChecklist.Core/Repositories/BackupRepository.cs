using Newtonsoft.Json;
using SimpleChecklist.Common;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.Core.Repositories
{
    public class BackupRepository
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Func<string, IDirectoryFilesReader> _directoryFrFunc;
        private readonly Func<string, IFile> _fileFunc;

        public BackupRepository(Func<string, IFile> fileFunc, Func<string, IDirectoryFilesReader> directoryFrFunc, IDateTimeProvider dateTimeProvider)
        {
            _fileFunc = fileFunc;
            _directoryFrFunc = directoryFrFunc;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateBackupAsync()
        {
            var appDataFile = _fileFunc(AppSettings.ApplicationDataFileName);
            if (!appDataFile.Exist)
            {
                return;
            }

            await RemoveOldBackupsAsync(int.Parse(AppSettings.BackupsLimit));
            var backupFile = _fileFunc($"{AppSettings.BackupsDir}\\{_dateTimeProvider.UtcNow:yyyy-MM-ddTHHmmssZ}{AppSettings.ApplicationDataFileName}");
            await appDataFile.CopyFileAsync(backupFile);
        }

        public async Task<bool> RestoreLastBackupAsync()
        {
            var backupsDir = _directoryFrFunc(AppSettings.BackupsDir);

            if (!backupsDir.Exist)
            {
                return false;
            }

            var backups = (await backupsDir.GetFilesAsync())
                .Where(file => file.NameWithExtension.EndsWith(AppSettings.ApplicationDataFileName))
                .OrderByDescending(file => file.NameWithExtension);

            foreach (var backup in backups)
            {
                var serializedData = await backup.ReadTextAsync();
                var fileData = JsonConvert.DeserializeObject<FileData>(serializedData);
                if (fileData?.ToDoItems?.Any() == true || fileData?.DoneItems?.Any() == true)
                {
                    await backup.CopyFileAsync(_fileFunc(AppSettings.ApplicationDataFileName));
                    return true;
                }
            }

            return false;
        }

        private async Task RemoveOldBackupsAsync(int limit)
        {
            var backupsDir = _directoryFrFunc(AppSettings.BackupsDir);

            if (!backupsDir.Exist)
            {
                return;
            }

            var backupsToRemove = (await backupsDir.GetFilesAsync())
                .Where(file => file.NameWithExtension.EndsWith(AppSettings.ApplicationDataFileName))
                .OrderByDescending(file => file.NameWithExtension)
                .Skip(limit - 1); // - 1 because we will create one more backup after this method is called

            foreach (var backupToRemove in backupsToRemove)
            {
                await backupToRemove.DeleteAsync();
            }
        }
    }
}