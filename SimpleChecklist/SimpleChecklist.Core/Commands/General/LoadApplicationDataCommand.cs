using System;
using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.General
{
    class LoadApplicationDataCommand : ICommand
    {
        private readonly Func<string, IFile> _fileFunc;
        private readonly MessagesStream _messagesStream;
        private readonly ApplicationData _appData;

        public LoadApplicationDataCommand(Func<string, IFile> fileFunc, MessagesStream messagesStream, ApplicationData appData)
        {
            _fileFunc = fileFunc;
            _messagesStream = messagesStream;
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            if (!_fileFunc(AppSettings.ApplicationDataFileName).Exist)
            {
                // Application data file does not exist so it is first run
                _messagesStream.PutToStream(new EventMessage(EventType.ApplicationDataLoadFinished));
                return;
            }

            var result = await _appData.LoadAsync();

            _messagesStream.PutToStream(result
                ? new EventMessage(EventType.ApplicationDataLoadFinished)
                : new EventMessage(EventType.ApplicationDataLoadError));
        }
    }
}