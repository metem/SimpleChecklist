using SimpleChecklist.Core.Interfaces.Utils;
using System;
using System.Threading.Tasks;

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
            var file = _fileFunc(AppSettings.ApplicationDataFileName);
            if (file.Exist)
            {
                await
                    file.CopyFileAsync(
                        _fileFunc(AppSettings.ApplicationDataFileName + AppSettings.PartialBackupFileExtension));
            }
        }
    }
}