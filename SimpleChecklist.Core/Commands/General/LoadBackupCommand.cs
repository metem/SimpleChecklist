using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    public class LoadBackupCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly ApplicationData _appData;

        public LoadBackupCommand(IDialogUtils dialogUtils, ApplicationData appData)
        {
            _dialogUtils = dialogUtils;
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            var file = await _dialogUtils.OpenFileDialogAsync(new[] {AppSettings.BackupFileExtension});

            if (file == null)
            {
                // Cancelled
                return;
            }

            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.LoadBackupConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                await _appData.LoadAsync(file);
            }
        }
    }
}