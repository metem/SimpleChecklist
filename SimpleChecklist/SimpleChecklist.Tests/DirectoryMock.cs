using SimpleChecklist.Common.Interfaces.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.Tests
{
    public class DirectoryMock : IDirectoryFilesReader
    {
        private readonly FilesContainer _filesContainer;

        public DirectoryMock(string dir, FilesContainer filesContainer)
        {
            Name = dir;
            _filesContainer = filesContainer;
        }

        public bool Exist => _filesContainer.Any(file => file.Key.StartsWith(Name));

        public string Name { get; }

        public string Path => Name;

        public Task<IEnumerable<IFile>> GetFilesAsync()
        {
            IEnumerable<KeyValuePair<string, string>> filesInDir
                = _filesContainer.Where(file => file.Key.StartsWith(Name));

            return Task.FromResult<IEnumerable<IFile>>(filesInDir.Select(file => new FileMock(file.Key, _filesContainer)));
        }
    }
}