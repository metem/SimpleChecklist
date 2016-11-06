using System;
using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Utils.Serializers;

namespace SimpleChecklist.Core.Commands.General
{
    public class CreateBackupCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly IFileApplicationRepository _fileApplicationRepository;

        public CreateBackupCommand(IDialogUtils dialogUtils, IFileApplicationRepository fileApplicationRepository)
        {
            _dialogUtils = dialogUtils;
            _fileApplicationRepository = fileApplicationRepository;
        }

        public async Task ExecuteAsync()
        {
            var data = XmlStringSerializer.Serialize(_fileApplicationRepository.ApplicationData);

            var file =
                await
                    _dialogUtils.SaveFileDialogAsync(
                        $"{AppSettings.BackupFileName}-{DateTime.Now:yy.MM.dd_HH_mm_ss}",
                        new[] { AppSettings.BackupFileExtension });

            if (file == null)
            {
                // Cancelled
                return;
            }

            try
            {
                await file.SaveTextAsync(data);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, ex.Message, AppTexts.Close);
            }
        }
    }
}