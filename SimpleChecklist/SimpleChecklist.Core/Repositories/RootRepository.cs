using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Core.Repositories
{
    public class RootRepository : IRepository
    {
        public Task<IEnumerable<ToDoItem>> GetToDoItemsAsync()
        {
            return Task.FromResult<IEnumerable<ToDoItem>>(new List<ToDoItem>());
        }

        public Task<IEnumerable<DoneItem>> GetDoneItemsAsync()
        {
            return Task.FromResult<IEnumerable<DoneItem>>(new List<DoneItem>());
        }

        public Task AddItemAsync(ToDoItem item)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> RemoveItemAsync(ToDoItem item)
        {
            return Task.FromResult(true);
        }

        public Task AddItemAsync(DoneItem item)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> RemoveItemAsync(DoneItem item)
        {
            return Task.FromResult(true);
        }

        public Task SetDoneItemsAsync(IEnumerable<DoneItem> doneItems)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetToDoItemsAsync(IEnumerable<ToDoItem> toDoItems)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> SaveChangesAsync()
        {
            return Task.FromResult(true);
        }
    }
}