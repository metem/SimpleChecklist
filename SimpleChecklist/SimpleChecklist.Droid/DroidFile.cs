using System;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    public class DroidFile : IFile
    {
        public DroidFile(string fileName)
        {
            Name = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }

        public DroidFile(FileInfo file)
        {
            Name = file.FullName;
        }

        public Task CreateAsync()
        {
            return Task.Run(() => File.Create(Name));
        }

        public Task<string> ReadTextAsync()
        {
            return Task.Run(() => File.ReadAllText(Name));
        }

        public Task<byte[]> ReadBytesAsync()
        {
            return Task.Run(() => File.ReadAllBytes(Name));
        }

        public Task CopyFileAsync(IFile destinationFile)
        {
            return Task.Run(() => File.Copy(Name, destinationFile.Name));
        }

        public string Name { get; }

        public bool Exist => File.Exists(Name);

        public Task SaveBytesAsync(byte[] content)
        {
            return Task.Run(() => File.WriteAllBytes(Name, content));
        }

        public Task SaveTextAsync(string content)
        {
            return Task.Run(() => File.WriteAllText(Name, content));
        }
    }
}