using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    class DroidDirectory : IDirectory
    {
        public DroidDirectory(string path)
        {
            Path = path;
        }

        public IEnumerable<IFile> GetFiles()
        {
            var files = Directory.EnumerateFiles(Path);
            return files.Select(file => new DroidFile(file));
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            var directories = Directory.EnumerateDirectories(Path);
            return directories.Select(directory => new DroidDirectory(directory));
        }

        public string Path { get; }

        public string Name => System.IO.Path.GetFileName(Path);

        public bool Exist => Directory.Exists(Path);
        public IDirectory GetChild(string name)
        {
            return new DroidDirectory(System.IO.Path.Combine(Path, name));
        }

        public IDirectory GetParent()
        {
            return new DroidDirectory(Directory.GetParent(Path).FullName);
        }
    }
}