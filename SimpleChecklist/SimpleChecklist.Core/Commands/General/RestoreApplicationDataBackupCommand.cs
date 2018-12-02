using System;
using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    internal class RestoreApplicationDataBackupCommand : ICommand
    {
        private readonly Func<string, IFile> _fileFunc;

        public RestoreApplicationDataBackupCommand(Func<string, IFile> fileFunc)
        {
            _fileFunc = fileFunc;
        }

        public async Task ExecuteAsync()
        {
            await Task.Run(() =>
            {
                var file = _fileFunc(AppSettings.ApplicationDataFileName + AppSettings.PartialBackupFileExtension);
                if (file.Exist)
                {
                    file.CopyFileAsync(_fileFunc(AppSettings.ApplicationDataFileName)).Wait();
                }
            });
        }
    }
}