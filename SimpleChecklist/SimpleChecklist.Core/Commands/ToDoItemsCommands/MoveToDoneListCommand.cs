using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class MoveToDoneListCommand : ICommand
    {
        private readonly IToDoItem _item;
        private readonly IApplicationRepository _applicationRepository;
        private readonly MessagesStream _messagesStream;

        public MoveToDoneListCommand(IToDoItem item, IApplicationRepository applicationRepository,
            MessagesStream messagesStream)
        {
            _item = item;
            _applicationRepository = applicationRepository;
            _messagesStream = messagesStream;
        }

        public async Task ExecuteAsync()
        {
            _applicationRepository.RemoveItem(_item);
            await Task.Run(() =>
            {
                _applicationRepository.AddItem(new DoneItem(_item));
                _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
            });
        }
    }
}