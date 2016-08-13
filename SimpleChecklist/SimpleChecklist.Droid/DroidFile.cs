using System;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    public class DroidFile : IFile
    {
        public string FullName { get; }

        public DroidFile(string fileName)
        {
            FullName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
        }

        public DroidFile(FileInfo file)
        {
            FullName = file.FullName;
        }

        public Task CreateAsync()
        {
            return Task.Run(() => File.Create(FullName));
        }

        public Task<string> ReadTextAsync()
        {
            return Task.Run(() => File.ReadAllText(FullName));
        }

        public Task<byte[]> ReadBytesAsync()
        {
            return Task.Run(() => File.ReadAllBytes(FullName));
        }

        public Task CopyFileAsync(IFile destinationFile)
        {
            return Task.Run(() => File.Copy(FullName, destinationFile.FullName));
        }

        public string Name => Path.GetFileName(FullName);

        public bool Exist => File.Exists(FullName);

        public Task SaveBytesAsync(byte[] content)
        {
            return Task.Run(() => File.WriteAllBytes(FullName, content));
        }

        public Task SaveTextAsync(string content)
        {
            return Task.Run(() => File.WriteAllText(FullName, content));
        }
    }
}