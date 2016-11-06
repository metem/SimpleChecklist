using System;
using System.Linq;
using System.Threading.Tasks;
using SimpleChecklist.Core.Entities;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    public class AddTasksFromTextFileCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly IFileApplicationRepository _fileApplicationRepository;

        public AddTasksFromTextFileCommand(IDialogUtils dialogUtils, IFileApplicationRepository fileApplicationRepository)
        {
            _dialogUtils = dialogUtils;
            _fileApplicationRepository = fileApplicationRepository;
        }

        public async Task ExecuteAsync()
        {
            var file = await _dialogUtils.OpenFileDialogAsync(new[] {AppSettings.TextFileExtension});

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
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, AppTexts.TextFileLoadError, AppTexts.Close);
                return;
            }

            if (string.IsNullOrEmpty(text))
                return;

            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.LoadTasksFromTextFileConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                text = text.Replace("\t", string.Empty).Replace("\r", string.Empty);
                var tasks = text.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                var tasksReversed = tasks.Reverse();
                foreach (var task in tasksReversed)
                {
                    _fileApplicationRepository.AddItem(new ToDoItem {Description = task});
                }
            }
        }
    }
}