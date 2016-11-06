using System.Threading.Tasks;
using SimpleChecklist.Core.Interfaces;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    public class AddToDoItemCommand : ICommand
    {
        private readonly IToDoItem _item;
        private readonly IApplicationRepository _applicationRepository;

        public AddToDoItemCommand(IToDoItem item, IApplicationRepository applicationRepository)
        {
            _item = item;
            _applicationRepository = applicationRepository;
        }

        public async Task ExecuteAsync()
        {
            _applicationRepository.AddItem(_item);
            await Task.Yield();
        }
    }
}