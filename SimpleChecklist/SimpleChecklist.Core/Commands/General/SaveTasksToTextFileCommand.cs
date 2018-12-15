using System;
using System.Linq;
using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.General
{
    public class SaveTasksToTextFileCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly ApplicationData _appData;

        public SaveTasksToTextFileCommand(IDialogUtils dialogUtils, ApplicationData appData)
        {
            _dialogUtils = dialogUtils;
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            var file = await _dialogUtils.SaveFileDialogAsync("list", new[] { AppSettings.TextFileExtension });

            if (file == null)
            {
                // Cancelled
                return;
            }

            var todolist = _appData.ToDoItems.Select(t => t.Data).Aggregate((t1, t2) => t1 + "\r\n" + t2);
            var donelist = _appData.DoneItems.Select(t => t.Data).Aggregate((t1, t2) => t1 + "\r\n" + t2);
            await file.SaveTextAsync("To do:\r\n\r\n" + todolist + "\r\n\r\nDone:\r\n\r\n" + donelist);
        }
    }
}