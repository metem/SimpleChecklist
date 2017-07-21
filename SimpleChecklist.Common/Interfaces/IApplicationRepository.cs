using System.Collections.Generic;

namespace SimpleChecklist.Common.Interfaces
{
    public interface IApplicationRepository
    {
        IEnumerable<IToDoItem> ToDoItems { get; }

        IEnumerable<IDoneItem> DoneItems { get; }

        int LoadPriority { get; }

        void AddItem(IToDoItem item);

        bool RemoveItem(IToDoItem item);

        void AddItem(IDoneItem item);

        bool RemoveItem(IDoneItem item);
    }
}