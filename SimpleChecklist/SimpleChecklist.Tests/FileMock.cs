using SimpleChecklist.Common.Interfaces.Utils;
using System.IO;
using System.Threading.Tasks;

namespace SimpleChecklist.Tests
{
    public class FileMock : IFile
    {
        private readonly FilesContainer _filesContainer;

        public FileMock(FilesContainer filesContainer)
        {
            NameWithExtension = "test.dat";
            _filesContainer = filesContainer;
        }

        public FileMock(string fileName, FilesContainer filesContainer)
        {
            NameWithExtension = fileName;
            _filesContainer = filesContainer;
        }

        public bool Exist => _filesContainer.ContainsKey(NameWithExtension);
        public string FilePath => string.Empty;
        public string NameWithExtension { get; }
        public string NameWithPath => Path.Combine(FilePath, NameWithExtension);

        public Task CopyFileAsync(IFile destinationFile)
        {
            if (Exist)
            {
                _filesContainer[destinationFile.NameWithExtension] = _filesContainer[NameWithExtension];
            }

            return Task.CompletedTask;
        }

        public Task CreateAsync()
        {
            _filesContainer[NameWithExtension] = string.Empty;
            return Task.CompletedTask;
        }

        public Task DeleteAsync()
        {
            _filesContainer.Remove(NameWithExtension);
            return Task.CompletedTask;
        }

        public Task<string> ReadTextAsync()
        {
            return Task.FromResult(_filesContainer[NameWithExtension]);
        }

        public Task SaveTextAsync(string content)
        {
            _filesContainer[NameWithExtension] = content;
            return Task.CompletedTask;
        }
    }
}