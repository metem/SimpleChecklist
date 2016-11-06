using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;

namespace SimpleChecklist.Core.Commands.General
{
    class SaveApplicationDataCommand : ICommand
    {
        private readonly IFileApplicationRepository _fileApplicationRepository;

        public SaveApplicationDataCommand(IFileApplicationRepository fileApplicationRepository)
        {
            _fileApplicationRepository = fileApplicationRepository;
        }

        public async Task ExecuteAsync()
        {
            await _fileApplicationRepository.SaveToFileAsync(AppSettings.ApplicationDataFileName);
        }
    }
}