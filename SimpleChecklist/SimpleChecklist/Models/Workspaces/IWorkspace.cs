using System.Threading.Tasks;

namespace SimpleChecklist.Models.Workspaces
{
    public interface IWorkspace
    {
        Task<bool> SaveCurrentStateAsync();

        Task<bool> LoadCurrentStateAsync();

        Task CreateBackup();

        Task RestoreBackup();
    }
}