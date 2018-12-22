using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class UpdateToDoItemCommand : ICommand
    {
        private readonly ToDoItem _item;
        protected internal string _newData;

        public UpdateToDoItemCommand(ToDoItem item, string newData)
        {
            _newData = newData;
            _item = item;
        }

        public Task ExecuteAsync()
        {
            _item.Data = _newData;
            return Task.FromResult(0);
        }
    }
}