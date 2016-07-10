using System;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    public class DroidFileUtils : IFileUtils
    {
        public async Task<string> LocalReadTextAsync(string fileName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            try
            {
                using (var streamReader = new StreamReader(Path.Combine(path, fileName)))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public Task<string> ReadTextAsync(object file)
        {
            throw new NotImplementedException();
        }

        public Task SaveTextAsync(object file, string content)
        {
            throw new NotImplementedException();
        }

        public async Task LocalSaveTextAsync(string fileName, string content)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            using (var streamWriter = new StreamWriter(Path.Combine(path, fileName), false))
            {
                await streamWriter.WriteAsync(content);
            }
        }

        public Task<byte[]> ReadBytesAsync(object file)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> LocalReadBytesAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task SaveBytesAsync(object file, byte[] content)
        {
            throw new NotImplementedException();
        }

        public Task LocalSaveBytesAsync(string fileName, byte[] content)
        {
            throw new NotImplementedException();
        }

        public Task CopyFileAsync(object sourceFile, object destinationFile)
        {
            throw new NotImplementedException();
        }

        public Task LocalCopyFileAsync(string sourceFileName, string destinationFileName)
        {
            throw new NotImplementedException();
        }
    }
}