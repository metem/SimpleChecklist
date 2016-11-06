using System.Threading.Tasks;
using SimpleChecklist.Core.Repositories.v1_3;

namespace SimpleChecklist.Core.Interfaces
{
    public interface IFileApplicationRepository : IApplicationRepository
    {
        Task<bool> LoadFromFileAsync(string fileName);

        Task<bool> SaveToFileAsync(string fileName);

        Task<bool> Load(ApplicationData applicationData);

        ApplicationData ApplicationData { get; }
    }
}