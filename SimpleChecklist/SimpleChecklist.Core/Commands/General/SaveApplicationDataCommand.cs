using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Commands.General
{
    class SaveApplicationDataCommand : ICommand
    {
        private readonly ApplicationData _appData;

        public SaveApplicationDataCommand(ApplicationData appData)
        {
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            await _appData.SaveAsync();
        }
    }
}