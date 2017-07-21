using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Common.Interfaces
{
    public interface IFileApplicationRepository : IApplicationRepository
    {
        Task<bool> LoadFromFileAsync(string fileName);

        Task<bool> SaveToFileAsync(string fileName);

        Task<bool> Load(ApplicationData applicationData);

        ApplicationData ApplicationData { get; }
    }
}