using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.DoneItemsCommands
{
    public class RemoveDoneItemCommand : ICommand
    {
        private readonly DoneItem _item;
        private readonly ApplicationData _appData;
        private readonly IDialogUtils _dialogUtils;
        private readonly MessagesStream _messagesStream;

        public RemoveDoneItemCommand(DoneItem item, ApplicationData appData,
            IDialogUtils dialogUtils, MessagesStream messagesStream)
        {
            _item = item;
            _appData = appData;
            _dialogUtils = dialogUtils;
            _messagesStream = messagesStream;
        }

        public async Task ExecuteAsync()
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.RemoveTaskConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                _appData.DoneItems.Remove(_item);
                _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
            }
        }
    }
}