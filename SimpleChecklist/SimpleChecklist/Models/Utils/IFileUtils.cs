using System.Threading.Tasks;

namespace SimpleChecklist.Models.Utils
{
    public interface IFileUtils
    {
        Task<string> ReadTextAsync(object file, bool useApplicationDataPath = false);

        Task SaveTextAsync(object file, string contents, bool useApplicationDataPath = false);

        Task<byte[]> ReadBytesAsync(object file, bool useApplicationDataPath = false);

        Task SaveBytesAsync(object file, byte[] contents, bool useApplicationDataPath = false);
    }
}