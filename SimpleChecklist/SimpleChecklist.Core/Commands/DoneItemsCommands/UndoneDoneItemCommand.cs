﻿using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Interfaces.Utils;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.DoneItemsCommands
{
    public class UndoneDoneItemCommand : ICommand
    {
        private readonly IDoneItem _item;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IDialogUtils _dialogUtils;
        private readonly MessagesStream _messagesStream;

        public UndoneDoneItemCommand(IDoneItem item, IApplicationRepository applicationRepository,
            IDialogUtils dialogUtils, MessagesStream messagesStream)
        {
            _item = item;
            _applicationRepository = applicationRepository;
            _dialogUtils = dialogUtils;
            _messagesStream = messagesStream;
        }

        public async Task ExecuteAsync()
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.UndoneTaskConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {

                _applicationRepository.AddItem((IToDoItem) _item);
                _applicationRepository.RemoveItem(_item);
                _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
            }
        }
    }
}