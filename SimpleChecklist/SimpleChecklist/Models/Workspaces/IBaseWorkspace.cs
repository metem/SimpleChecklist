using System.Threading.Tasks;

namespace SimpleChecklist.Models.Workspaces
{
    public interface IBaseWorkspace
    {
        ViewsId ViewId { get; }

        Task<bool> SaveCurrentStateAsync();

        Task<bool> LoadCurrentStateAsync();
    }
}