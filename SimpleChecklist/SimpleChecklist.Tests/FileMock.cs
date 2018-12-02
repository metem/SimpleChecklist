using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Tests
{
    public class FileMock : IFile
    {
        public string TextData { get; private set; }

        public FileMock(bool exist = true, string textData = "")
        {
            Exist = exist;
            TextData = textData;
        }

        public string Name => "testFile.dat";
        public string FullName => "testFile.dat";
        public bool Exist { get; }

        public Task CreateAsync()
        {
            return Task.FromResult(0);
        }

        public Task<string> ReadTextAsync()
        {
            return Task.FromResult(TextData);
        }

        public Task SaveTextAsync(string content)
        {
            return Task.Run(() => TextData = content);
        }

        public Task<byte[]> ReadBytesAsync()
        {
            return Task.FromResult(new byte[0]);
        }

        public Task SaveBytesAsync(byte[] content)
        {
            return Task.FromResult(0);
        }

        public Task CopyFileAsync(IFile destinationFile)
        {
            return Task.FromResult(0);
        }
    }
}