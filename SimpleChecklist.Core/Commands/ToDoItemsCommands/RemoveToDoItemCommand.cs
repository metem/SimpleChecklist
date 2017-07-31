using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class RemoveToDoItemCommand : ICommand
    {
        private readonly ToDoItem _item;
        private readonly ApplicationData _appData;
        private readonly IDialogUtils _dialogUtils;

        public RemoveToDoItemCommand(ToDoItem item, ApplicationData appData,
            IDialogUtils dialogUtils)
        {
            _item = item;
            _appData = appData;
            _dialogUtils = dialogUtils;
        }

        public async Task ExecuteAsync()
        {
            var accepted =
                await _dialogUtils.DisplayAlertAsync(
                    AppTexts.Alert,
                    AppTexts.RemoveTaskConfirmationText,
                    AppTexts.Yes,
                    AppTexts.No);

            if (accepted)
            {
                _appData.ToDoItems.Remove(_item);
            }
        }
    }
}