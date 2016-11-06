using System;
using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    internal class CreateApplicationDataBackupCommand : ICommand
    {
        private readonly Func<string, IFile> _fileFunc;

        public CreateApplicationDataBackupCommand(Func<string, IFile> fileFunc)
        {
            _fileFunc = fileFunc;
        }

        public async Task ExecuteAsync()
        {
            await _fileFunc(AppSettings.ApplicationDataFileName)
                .CopyFileAsync(_fileFunc(AppSettings.ApplicationDataFileName + AppSettings.PartialBackupFileExtension));
        }
    }
}