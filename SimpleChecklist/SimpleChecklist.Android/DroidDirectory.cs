using SimpleChecklist.Common.Interfaces.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.Droid
{
    internal class DroidDirectory : IDirectory
    {
        public DroidDirectory(string path)
        {
            Path = path;
        }

        public bool Exist => Directory.Exists(Path);

        public string Name => System.IO.Path.GetFileName(Path);

        public string Path { get; }

        public IDirectory GetChild(string name)
        {
            return new DroidDirectory(System.IO.Path.Combine(Path, name));
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            var directories = Directory.EnumerateDirectories(Path);
            return directories.Select(directory => new DroidDirectory(directory));
        }

        public Task<IEnumerable<IFile>> GetFilesAsync()
        {
            var files = Directory.EnumerateFiles(Path);
            return Task.FromResult(files.Select(file => (IFile)new DroidFile(file)));
        }

        public IDirectory GetParent()
        {
            var parent = Directory.GetParent(Path);
            return parent == null ? this : new DroidDirectory(parent.FullName);
        }
    }
}