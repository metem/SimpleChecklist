using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleChecklist.Properties;

namespace SimpleChecklist.Models.Collections
{
    public class TaskListObservableCollection : INotifyPropertyChanged
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
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}