using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.General
{
    internal class AskUserIfContinueAfterInitializationFailureCommand : ICommand
    {
        private readonly MessagesStream _messagesStream;
        private readonly IDialogUtils _dialogUtils;

        public AskUserIfContinueAfterInitializationFailureCommand(MessagesStream messagesStream, IDialogUtils dialogUtils)
        {
            _messagesStream = messagesStream;
            _dialogUtils = dialogUtils;
        }

        public async Task ExecuteAsync()
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Error,
                AppTexts.LoadErrorConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            _messagesStream.PutToStream(accepted
                ? new EventMessage(EventType.InitializationCancelled)
                : new EventMessage(EventType.InitializationFromBackupRequested));
        }
    }
}