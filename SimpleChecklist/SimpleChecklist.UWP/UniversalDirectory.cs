using SimpleChecklist.Common.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SimpleChecklist.Universal
{
    public class UniversalDirectory : IDirectoryFilesReader
    {
        private IStorageFolder _storageFolder;

        public UniversalDirectory(string subFolder)
        {
            Task.Run(async () =>
            {
                _storageFolder = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(subFolder, CreationCollisionOption.OpenIfExists);
            }).Wait();
            Name = subFolder;
        }

        public UniversalDirectory(IStorageFolder storageFolder)
        {
            _storageFolder = storageFolder;
            Name = _storageFolder?.Name;
        }

        public bool Exist => _storageFolder != null;

        public string Name { get; }

        public string Path => Name;

        public async Task<IEnumerable<IFile>> GetFilesAsync()
        {
            IReadOnlyList<StorageFile> files = await _storageFolder.GetFilesAsync();
            return files.Select(file => new UniversalFile(file));
        }
    }
}