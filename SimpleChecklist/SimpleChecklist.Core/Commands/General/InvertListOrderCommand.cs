using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    public class InvertListOrderCommand : ICommand
    {
        private readonly ApplicationData _appData;

        public InvertListOrderCommand(ApplicationData appData)
        {
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            _appData.InvertToDoListOrder();
            await Task.FromResult(0);
        }
    }
}