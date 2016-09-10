using System.Threading.Tasks;

namespace SimpleChecklist.Models.Utils
{
    public interface IFile
    {
        string Name { get; }

        string FullName { get; }

        bool Exist { get; }

        Task CreateAsync();

        Task<string> ReadTextAsync();

        Task SaveTextAsync(string content);

        Task<byte[]> ReadBytesAsync();

        Task SaveBytesAsync(byte[] content);

        Task CopyFileAsync(IFile destinationFile);
    }
}