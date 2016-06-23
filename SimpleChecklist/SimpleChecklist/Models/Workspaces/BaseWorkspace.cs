using System.Threading.Tasks;

namespace SimpleChecklist.Models.Workspaces
{
    public class BaseWorkspace : IBaseWorkspace
    {
        public BaseWorkspace(ViewsId viewId)
        {
            ViewId = viewId;
        }

        public ViewsId ViewId { get; }

        public virtual async Task<bool> SaveCurrentStateAsync()
        {
            return await Task.Run(() => true);
        }

        public virtual async Task<bool> LoadCurrentStateAsync()
        {
            return await Task.Run(() => true);
        }
    }
}