using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Common.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<ToDoItem>> GetToDoItemsAsync();

        Task<IEnumerable<DoneItem>> GetDoneItemsAsync();

        Task AddItemAsync(ToDoItem item);

        Task<bool> RemoveItemAsync(ToDoItem item);

        Task AddItemAsync(DoneItem item);

        Task<bool> RemoveItemAsync(DoneItem item);

        Task<bool> SaveChangesAsync();

        Task SetDoneItemsAsync(IEnumerable<DoneItem> doneItems);

        Task SetToDoItemsAsync(IEnumerable<ToDoItem> toDoItems);

        Task SetSettingsAsync(Settings settings);

        Task<Settings> GetSettingsAsync();
    }
}