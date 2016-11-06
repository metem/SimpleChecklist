using System;
using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Repositories.v1_3;
using SimpleChecklist.Core.Utils.Serializers;

namespace SimpleChecklist.Core.Commands.General
{
    public class LoadBackupCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly IFileApplicationRepository _fileApplicationRepository;

        public LoadBackupCommand(IDialogUtils dialogUtils, IFileApplicationRepository fileApplicationRepository)
        {
            _dialogUtils = dialogUtils;
            _fileApplicationRepository = fileApplicationRepository;
        }

        public async Task ExecuteAsync()
        {
            var file = await _dialogUtils.OpenFileDialogAsync(new[] {AppSettings.BackupFileExtension});

            if (file == null)
            {
                // Cancelled
                return;
            }

            string text;

            try
            {
                text = file.Exist ? await file.ReadTextAsync() : string.Empty;
            }
            catch (ArgumentOutOfRangeException)
            {
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, AppTexts.BackupLoadError, AppTexts.Close);
                return;
            }

            if (string.IsNullOrEmpty(text))
                return;

            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.LoadBackupConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                try
                {
                    var data = XmlStringSerializer.Deserialize<ApplicationData>(text);

                    if (data != null)
                    {
                        await _fileApplicationRepository.Load(data);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}