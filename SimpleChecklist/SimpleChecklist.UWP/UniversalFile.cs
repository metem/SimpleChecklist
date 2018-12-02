using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Universal
{
    public class UniversalFile : IFile
    {
        private IStorageFile _storageFile;

        public UniversalFile(string fileName)
        {
            SetStorageFileAsync(fileName).Wait();
            Name = fileName;
        }

        private async Task SetStorageFileAsync(string fileName)
        {
            var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName).AsTask()
                .ConfigureAwait(false);
            _storageFile = storageItem == null
                ? null
                : await ApplicationData.Current.LocalFolder.GetFileAsync(fileName).AsTask().ConfigureAwait(false);
        }

        public UniversalFile(IStorageFile storageFile)
        {
            _storageFile = storageFile;
            Name = _storageFile?.Name;
        }

        public async Task CreateAsync()
        {
            _storageFile =
                await ApplicationData.Current.LocalFolder.CreateFileAsync(Name, CreationCollisionOption.ReplaceExisting)
                    .AsTask().ConfigureAwait(false);
        }

        public async Task<string> ReadTextAsync()
        {
            return await FileIO.ReadTextAsync(_storageFile, UnicodeEncoding.Utf8).AsTask().ConfigureAwait(false);
        }

        public async Task SaveTextAsync(string content)
        {
            await FileIO.WriteTextAsync(_storageFile, content, UnicodeEncoding.Utf8).AsTask().ConfigureAwait(false);
        }

        public async Task<byte[]> ReadBytesAsync()
        {
            var result = await FileIO.ReadBufferAsync(_storageFile).AsTask().ConfigureAwait(false);
            return result.ToArray();
        }

        public async Task SaveBytesAsync(byte[] content)
        {
            await FileIO.WriteBytesAsync(_storageFile, content).AsTask().ConfigureAwait(false);
        }

        public async Task CopyFileAsync(IFile destinationFile)
        {
            if (destinationFile is UniversalFile universalFile)
            {
                if (!universalFile.Exist)
                {
                    await universalFile.CreateAsync().ConfigureAwait(false);
                }

                await
                    _storageFile.CopyAsync(await universalFile.GetParentAsync(), destinationFile.Name,
                        NameCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
            }
        }

        public string Name { get; }

        public string FullName => Name;

        public bool Exist => _storageFile != null;

        private async Task<IStorageFolder> GetParentAsync()
        {
            if (_storageFile is StorageFile storageFile)
                return await storageFile.GetParentAsync().AsTask().ConfigureAwait(false);

            return null;
        }
    }
}
