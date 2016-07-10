using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Universal
{
    public class UniversalFileUtils : IFileUtils
    {
        public async Task<string> ReadTextAsync(object file)
        {
            var storageFile = file as StorageFile;

            if (storageFile != null)
                return await FileIO.ReadTextAsync(storageFile);

            return null;
        }

        public async Task<string> LocalReadTextAsync(string fileName)
        {
            var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            if (storageItem == null)
                throw new FileNotFoundException();

            var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            if (storageFile == null)
                return null;

            return await FileIO.ReadTextAsync(storageFile);
        }

        public async Task SaveTextAsync(object file, string content)
        {
            var storageFile = file as StorageFile;

            if (storageFile != null)
                await FileIO.WriteTextAsync(storageFile, content);
        }

        public async Task LocalSaveTextAsync(string fileName, string content)
        {
            var storageFile = await
                ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                    CreationCollisionOption.ReplaceExisting);

            if (storageFile != null)
                await FileIO.WriteTextAsync(storageFile, content);
        }

        public async Task<byte[]> ReadBytesAsync(object file)
        {
            var storageFile = file as StorageFile;

            if (storageFile != null)
            {
                var result = await FileIO.ReadBufferAsync(storageFile);
                return result.ToArray();
            }

            return null;
        }

        public async Task<byte[]> LocalReadBytesAsync(string fileName)
        {
            var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            if (storageItem == null)
                throw new FileNotFoundException();

            var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            if (storageFile == null) return null;

            var result = await FileIO.ReadBufferAsync(storageFile);
            return result.ToArray();
        }

        public async Task SaveBytesAsync(object file, byte[] content)
        {
            var storageFile = file as StorageFile;

            if (storageFile != null)
                await FileIO.WriteBytesAsync(storageFile, content);
        }

        public async Task LocalSaveBytesAsync(string fileName, byte[] content)
        {
            var storageFile = await
                ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                    CreationCollisionOption.ReplaceExisting);

            if (storageFile != null)
                await FileIO.WriteBytesAsync(storageFile, content);
        }

        public async Task CopyFileAsync(object sourceFile, object destinationFile)
        {
            var startFile = sourceFile as StorageFile;
            var endFile = destinationFile as StorageFile;
            if (endFile != null && startFile != null)
                await
                    startFile.CopyAsync(await endFile.GetParentAsync(), endFile.Name,
                        NameCollisionOption.ReplaceExisting);
        }

        public async Task LocalCopyFileAsync(string sourceFileName, string destinationFileName)
        {
            var storageFile =
                await
                    StorageFile.GetFileFromPathAsync(Path.Combine(ApplicationData.Current.LocalFolder.Path,
                        sourceFileName));

            await
                storageFile.CopyAsync(ApplicationData.Current.LocalFolder, destinationFileName,
                    NameCollisionOption.ReplaceExisting);
        }
    }
}