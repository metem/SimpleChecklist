using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class RemoveToDoItemCommand : ICommand
    {
        private readonly IToDoItem _item;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IDialogUtils _dialogUtils;

        public RemoveToDoItemCommand(IToDoItem item, IApplicationRepository applicationRepository,
            IDialogUtils dialogUtils)
        {
            _item = item;
            _applicationRepository = applicationRepository;
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
                _applicationRepository.RemoveItem(_item);
            }
        }
    }
}