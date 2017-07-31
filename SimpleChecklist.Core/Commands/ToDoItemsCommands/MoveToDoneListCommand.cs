using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class MoveToDoneListCommand : ICommand
    {
        private readonly ToDoItem _item;
        private readonly ApplicationData _appData;
        private readonly MessagesStream _messagesStream;

        public MoveToDoneListCommand(ToDoItem item, ApplicationData appData,
            MessagesStream messagesStream)
        {
            _item = item;
            _appData = appData;
            _messagesStream = messagesStream;
        }

        public Task ExecuteAsync()
        {
            _appData.ToDoItems.Remove(_item);
            _appData.DoneItems.Add(new DoneItem(_item));
            _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
            return Task.FromResult(0);
        }
    }
}