using SimpleChecklist.Common.Interfaces.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SimpleChecklist.Universal
{
    public class UniversalFile : IFile
    {
        private IStorageFile _storageFile;

        public UniversalFile(string fileName)
        {
            var fullFileName = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, fileName);
            FilePath = Path.GetDirectoryName(fullFileName);
            NameWithExtension = Path.GetFileName(fullFileName);

            try
            {
                Task.Run(SetStorageFileAsync).Wait();
            }
            catch
            {
                // ignore, files doesn't exist.
            }
        }

        public UniversalFile(IStorageFile storageFile)
        {
            _storageFile = storageFile;

            FilePath = storageFile?.Path;
            NameWithExtension = _storageFile?.Name;
        }

        public bool Exist => _storageFile != null;

        public string FilePath { get; }

        public string NameWithExtension { get; }

        public string NameWithPath => Path.Combine(FilePath, NameWithExtension);

        public async Task CopyFileAsync(IFile destinationFile)
        {
            if (_storageFile == null)
            {
                return;
            }

            StorageFolder destinationStorageFolder = await StorageFolder.GetFolderFromPathAsync(destinationFile.FilePath);
            await _storageFile.CopyAsync(destinationStorageFolder, destinationFile.NameWithExtension, NameCollisionOption.ReplaceExisting);
        }

        public async Task CreateAsync()
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(FilePath);
            _storageFile = await storageFolder.CreateFileAsync(NameWithExtension, CreationCollisionOption.ReplaceExisting);
        }

        public Task DeleteAsync()
        {
            return _storageFile.DeleteAsync().AsTask();
        }

        public Task<string> ReadTextAsync()
        {
            return FileIO.ReadTextAsync(_storageFile, UnicodeEncoding.Utf8).AsTask();
        }

        public Task SaveTextAsync(string content)
        {
            return FileIO.WriteTextAsync(_storageFile, content, UnicodeEncoding.Utf8).AsTask();
        }

        private async Task SetStorageFileAsync()
        {
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(FilePath);
            _storageFile = await storageFolder.GetFileAsync(NameWithExtension);
        }
    }
}