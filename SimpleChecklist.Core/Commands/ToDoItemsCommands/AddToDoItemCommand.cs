using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class AddToDoItemCommand : ICommand
    {
        private readonly ToDoItem _item;
        private readonly ApplicationData _appData;

        public AddToDoItemCommand(ToDoItem item, ApplicationData appData)
        {
            _item = item;
            _appData = appData;
        }

        public Task ExecuteAsync()
        {
            _appData.ToDoItems.Add(_item);
            return Task.FromResult(0);
        }
    }
}