using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleChecklist.LegacyDataRepository.Models.Collections
{
    public class TaskListObservableCollection
    {
        private ObservableCollection<ToDoItem> _toDoItems;

        public TaskListObservableCollection()
        {
            ToDoItems = new ObservableCollection<ToDoItem>();
        }

        public ObservableCollection<ToDoItem> ToDoItems
        {
            get { return _toDoItems; }
            private set
            {
                _toDoItems = value;
            }
        }

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
            var doneItems = _toDoItems.LastOrDefault(doneItem => doneItem.CreationDateTime > item.CreationDateTime);
            var index = doneItems == null ? 0 : _toDoItems.IndexOf(doneItems) + 1;
            _toDoItems.Insert(index, item);
        }
    }
}