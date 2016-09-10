using System.Threading.Tasks;
using SimpleChecklist.Views;

namespace SimpleChecklist.Models.Workspaces
{
    public interface IBaseWorkspace
    {
        ViewsId ViewId { get; }

        Task<bool> SaveCurrentStateAsync();

        Task<bool> LoadCurrentStateAsync();

        Task CreateBackup();

        Task RestoreBackup();
    }
}