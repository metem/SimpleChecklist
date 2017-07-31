using System;
using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    public class CreateBackupCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly ApplicationData _appData;

        public CreateBackupCommand(IDialogUtils dialogUtils, ApplicationData appData)
        {
            _dialogUtils = dialogUtils;
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            var file =
                await
                    _dialogUtils.SaveFileDialogAsync(
                        $"{AppSettings.BackupFileName}-{DateTime.Now:yy.MM.dd_HH_mm_ss}",
                        new[] {AppSettings.BackupFileExtension});

            if (file == null)
            {
                // Cancelled
                return;
            }

            await _appData.SaveAsync(file);
        }
    }
}