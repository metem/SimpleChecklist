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
            Task.Run(async () => await SetStorageFileAsync(fileName)).Wait();
            Name = fileName;
        }

        private async Task SetStorageFileAsync(string fileName)
        {
            var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            _storageFile = storageItem == null
                ? null
                : await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
        }

        public UniversalFile(IStorageFile storageFile)
        {
            _storageFile = storageFile;
            Name = _storageFile?.Name;
        }

        public async Task CreateAsync()
        {
            _storageFile =
                await ApplicationData.Current.LocalFolder.CreateFileAsync(Name, CreationCollisionOption.ReplaceExisting);
        }

        public async Task<string> ReadTextAsync()
        {
            return await FileIO.ReadTextAsync(_storageFile, UnicodeEncoding.Utf8);
        }

        public async Task SaveTextAsync(string content)
        {
            await FileIO.WriteTextAsync(_storageFile, content, UnicodeEncoding.Utf8);
        }

        public async Task<byte[]> ReadBytesAsync()
        {
            var result = await FileIO.ReadBufferAsync(_storageFile);
            return result.ToArray();
        }

        public async Task SaveBytesAsync(byte[] content)
        {
            await FileIO.WriteBytesAsync(_storageFile, content);
        }

        public async Task CopyFileAsync(IFile destinationFile)
        {
            var universalFile = destinationFile as UniversalFile;
            if (universalFile != null)
            {
                if (!universalFile.Exist)
                {
                    await universalFile.CreateAsync();
                }

                await
                    _storageFile.CopyAsync(await universalFile.GetParentAsync(), destinationFile.Name,
                        NameCollisionOption.ReplaceExisting);
            }
        }

        public string Name { get; }

        public string FullName { get; }

        public bool Exist => _storageFile != null;

        private async Task<IStorageFolder> GetParentAsync()
        {
            var storageFile = _storageFile as StorageFile;
            if (storageFile != null)
                return await storageFile.GetParentAsync();

            return null;
        }
    }
}