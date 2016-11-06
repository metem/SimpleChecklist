using System;
using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.General
{
    class LoadApplicationDataCommand : ICommand
    {
        private readonly Func<string, IFile> _fileFunc;
        private readonly IFileApplicationRepository _fileApplicationRepository;
        private readonly MessagesStream _messagesStream;

        public LoadApplicationDataCommand(Func<string,IFile> fileFunc, IFileApplicationRepository fileApplicationRepository, MessagesStream messagesStream)
        {
            _fileFunc = fileFunc;
            _fileApplicationRepository = fileApplicationRepository;
            _messagesStream = messagesStream;
        }

        public async Task ExecuteAsync()
        {
            if (!_fileFunc(AppSettings.ApplicationDataFileName).Exist)
            {
                // Application data file does not exist so it is first run
                _messagesStream.PutToStream(new EventMessage(EventType.ApplicationDataLoadFinished));
                return;
            }

            bool result;
            try
            {
                result = await _fileApplicationRepository.LoadFromFileAsync(AppSettings.ApplicationDataFileName);
            }
            catch (Exception)
            {
                result = false;
            }

            _messagesStream.PutToStream(result
                ? new EventMessage(EventType.ApplicationDataLoadFinished)
                : new EventMessage(EventType.ApplicationDataLoadError));
        }
    }
}