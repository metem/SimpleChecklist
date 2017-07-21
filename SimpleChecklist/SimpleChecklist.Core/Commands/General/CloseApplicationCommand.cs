using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    internal class CloseApplicationCommand : ICommand
    {
        private readonly IAppUtils _appUtils;

        public CloseApplicationCommand(IAppUtils appUtils)
        {
            _appUtils = appUtils;
        }

        public async Task ExecuteAsync()
        {
            await Task.Run(() => _appUtils.Close());
        }
    }
}