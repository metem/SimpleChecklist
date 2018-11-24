using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleChecklist.LegacyDataRepository.Models.Collections
{
    public class TaskListObservableCollection
    {
        public TaskListObservableCollection()
        {
            ToDoItems = new ObservableCollection<ToDoItem>();
        }

        public ObservableCollection<ToDoItem> ToDoItems { get; private set; }

        public void Clear()
        {
            ToDoItems.Clear();
        }

        public void Load(ObservableCollection<ToDoItem> data)
        {
            ToDoItems = data;
        }

        public void Add(string text)
        {
            var toDoItem = new ToDoItem {Description = text};
            Add(toDoItem);
        }

        public void Add(ToDoItem item)
        {
            var doneItems = ToDoItems.LastOrDefault(doneItem => doneItem.CreationDateTime > item.CreationDateTime);
            var index = doneItems == null ? 0 : ToDoItems.IndexOf(doneItems) + 1;
            ToDoItems.Insert(index, item);
        }
    }
}