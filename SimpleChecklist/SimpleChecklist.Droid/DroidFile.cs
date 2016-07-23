using System;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    public class DroidFile : IFile
    {
        public async Task CreateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> ReadTextAsync()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            try
            {
                using (var streamReader = new StreamReader(Path.Combine(path, Name)))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<byte[]> ReadBytesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CopyFileAsync(IFile destinationFile)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public bool Exist { get; }

        public async Task SaveBytesAsync(byte[] content)
        {
            throw new NotImplementedException();
        }

        public async Task SaveTextAsync(string content)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            using (var streamWriter = new StreamWriter(Path.Combine(path, Name), false))
            {
                await streamWriter.WriteAsync(content);
            }
        }
    }
}