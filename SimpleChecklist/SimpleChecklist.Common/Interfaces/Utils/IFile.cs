using System.Threading.Tasks;

namespace SimpleChecklist.Common.Interfaces.Utils
{
    public interface IFile
    {
        bool Exist { get; }
        string FilePath { get; }
        string NameWithExtension { get; }
        string NameWithPath { get; }

        Task CopyFileAsync(IFile destinationFile);

        Task CreateAsync();

        Task DeleteAsync();

        Task<string> ReadTextAsync();

        Task SaveTextAsync(string content);
    }
}