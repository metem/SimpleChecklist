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
        public async Task<string> ReadTextAsync(object file, bool useApplicationDataPath = false)
        {
            StorageFile storageFile = null;

            if (useApplicationDataPath)
            {
                var fileName = file as string;
                if (fileName != null)
                {
                    var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
                    if (storageItem == null)
                        throw new FileNotFoundException();

                    storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                    if (storageFile == null)
                        return null;
                }
            }
            else
            {
                storageFile = file as StorageFile;
            }

            if (storageFile != null)
                return await FileIO.ReadTextAsync(storageFile);

            return null;
        }

        public async Task SaveTextAsync(object file, string contents, bool useApplicationDataPath = false)
        {
            StorageFile storageFile = null;

            if (useApplicationDataPath)
            {
                var fileName = file as string;
                if (fileName != null)
                {
                    storageFile =
                        await
                            ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                                CreationCollisionOption.ReplaceExisting);
                }
            }
            else
            {
                storageFile = file as StorageFile;
            }

            if (storageFile != null)
                await FileIO.WriteTextAsync(storageFile, contents);
        }

        public async Task<byte[]> ReadBytesAsync(object file, bool useApplicationDataPath = false)
        {
            StorageFile storageFile = null;

            if (useApplicationDataPath)
            {
                var fileName = file as string;
                if (fileName != null)
                {
                    var storageItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
                    if (storageItem == null)
                        throw new FileNotFoundException();

                    storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                    if (storageFile == null)
                        return null;
                }
            }
            else
            {
                storageFile = file as StorageFile;
            }

            if (storageFile != null)
            {
                var result = await FileIO.ReadBufferAsync(storageFile);
                return result.ToArray();
            }

            return null;
        }

        public async Task SaveBytesAsync(object file, byte[] contents, bool useApplicationDataPath = false)
        {
            StorageFile storageFile = null;

            if (useApplicationDataPath)
            {
                var fileName = file as string;
                if (fileName != null)
                {
                    storageFile =
                        await
                            ApplicationData.Current.LocalFolder.CreateFileAsync(fileName,
                                CreationCollisionOption.ReplaceExisting);
                }
            }
            else
            {
                storageFile = file as StorageFile;
            }

            if (storageFile != null)
                await FileIO.WriteBytesAsync(storageFile, contents);
        }
    }
}