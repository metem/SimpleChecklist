using SimpleChecklist.Common.Interfaces.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpleChecklist.Droid
{
    public class DroidFile : IFile
    {
        public DroidFile(string fileName)
        {
            var fullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            FilePath = Path.GetDirectoryName(fullFileName);
            NameWithExtension = Path.GetFileName(fullFileName);
        }

        public DroidFile(FileInfo file)
        {
            FilePath = file.DirectoryName;
            NameWithExtension = file.Name;
        }

        public bool Exist => File.Exists(NameWithPath);
        public string FilePath { get; }
        public string NameWithExtension { get; }
        public string NameWithPath => Path.Combine(FilePath, NameWithExtension);

        public Task CopyFileAsync(IFile destinationFile)
        {
            File.Copy(NameWithPath, destinationFile.NameWithPath, true);
            return Task.CompletedTask;
        }

        public Task CreateAsync()
        {
            var fileStream = File.Open(NameWithPath, FileMode.OpenOrCreate, FileAccess.Read);
            fileStream.Close();
            return Task.CompletedTask;
        }

        public Task DeleteAsync()
        {
            File.Delete(NameWithPath);
            return Task.CompletedTask;
        }

        public Task<string> ReadTextAsync()
        {
            return File.ReadAllTextAsync(NameWithPath);
        }

        public Task SaveTextAsync(string content)
        {
            return File.WriteAllTextAsync(NameWithPath, content);
        }
    }
}