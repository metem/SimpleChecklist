using Newtonsoft.Json;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.DTOs;
using System;
using System.Threading.Tasks;

namespace SimpleChecklist.Core.Repositories
{
    public class FileDataRepository
    {
        private readonly Func<string, IFile> _fileFunc;

        public FileDataRepository(Func<string, IFile> fileFunc)
        {
            _fileFunc = fileFunc;
        }

        public bool ApplicationDataExists()
        {
            return _fileFunc(AppSettings.ApplicationDataFileName).Exist;
        }

        public async Task<FileData> GetFileDataAsync()
        {
            var file = _fileFunc(AppSettings.ApplicationDataFileName);
            var serializedData = await file.ReadTextAsync();
            return JsonConvert.DeserializeObject<FileData>(serializedData);
        }

        public async Task UpdateFileDataAsync(FileData fileData)
        {
            var serializedData = JsonConvert.SerializeObject(fileData);
            var file = _fileFunc(AppSettings.ApplicationDataFileName);
            await file.CreateAsync();
            await file.SaveTextAsync(serializedData);
        }
    }
}