using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces;
using SimpleChecklist.Common.Interfaces.Utils;

namespace SimpleChecklist.Common.Entities
{
    public class ApplicationData : INotifyPropertyChanged
    {
        private readonly IRepository _repository;
        private ObservableCollection<DoneItem> _doneItems = new ObservableCollection<DoneItem>();
        private ObservableCollection<ToDoItem> _toDoItems = new ObservableCollection<ToDoItem>();

        public ApplicationData(IRepository repository)
        {
            _repository = repository;
        }

        public ObservableCollection<DoneItem> DoneItems
        {
            get { return _doneItems; }
            private set
            {
                if (Equals(value, _doneItems)) return;
                _doneItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ToDoItem> ToDoItems
        {
            get { return _toDoItems; }
            private set
            {
                if (Equals(value, _toDoItems)) return;
                _toDoItems = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> SaveAsync()
        {
            await _repository.SetToDoItemsAsync(ToDoItems);
            await _repository.SetDoneItemsAsync(DoneItems);

            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> LoadAsync()
        {
            var toDoItems = await _repository.GetToDoItemsAsync();
            var doneItems = await _repository.GetDoneItemsAsync();

            ToDoItems = new ObservableCollection<ToDoItem>(toDoItems);
            DoneItems = new ObservableCollection<DoneItem>(doneItems);

            return toDoItems != null && doneItems != null;
        }

        public async Task<bool> LoadAsync(IFile file)
        {
            //TODO: backup load
            return true;
        }

        public async Task<bool> SaveAsync(IFile file)
        {
            //TODO: backup save
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}