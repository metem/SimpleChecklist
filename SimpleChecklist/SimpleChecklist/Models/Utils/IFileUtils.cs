using System.Threading.Tasks;

namespace SimpleChecklist.Models.Utils
{
    public interface IFileUtils
    {
        Task<string> LocalReadTextAsync(string fileName);

        Task<string> ReadTextAsync(object file);

        Task SaveTextAsync(object file, string content);

        Task LocalSaveTextAsync(string fileName, string content);

        Task<byte[]> ReadBytesAsync(object file);

        Task<byte[]> LocalReadBytesAsync(string fileName);

        Task SaveBytesAsync(object file, byte[] content);

        Task LocalSaveBytesAsync(string fileName, byte[] content);

        Task CopyFileAsync(object sourceFile, object destinationFile);

        Task LocalCopyFileAsync(string sourceFileName, string destinationFileName);
    }
}