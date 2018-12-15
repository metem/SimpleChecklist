using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Common.Entities
{
    public class ApplicationData : INotifyPropertyChanged
    {
        private readonly IRepository _repository;
        private ObservableCollection<DoneItem> _doneItems = new ObservableCollection<DoneItem>();
        private ObservableCollection<ToDoItem> _toDoItems = new ObservableCollection<ToDoItem>();
        private Settings _settings = new Settings();

        public Settings Settings
        {
            get
            {
                return _settings;
            }
            private set
            {
                if (Equals(value, _settings)) return;
                _settings = value;
                OnPropertyChanged();
            }
        }

        public bool ToDoListInverted { get; private set; }

        public void InvertToDoListOrder()
        {
            ToDoItems = new ObservableCollection<ToDoItem>(ToDoItems.Reverse());
            ToDoListInverted = !ToDoListInverted;
        }

        public ApplicationData(IRepository repository)
        {
            _repository = repository;
        }

        public ObservableCollection<DoneItem> DoneItems
        {
            get => _doneItems;
            set
            {
                if (Equals(value, _doneItems)) return;
                _doneItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ToDoItem> ToDoItems
        {
            get => _toDoItems;
            set
            {
                if (Equals(value, _toDoItems)) return;
                _toDoItems = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> SaveAsync()
        {
            if (ToDoListInverted)
            {
                ToDoItems = new ObservableCollection<ToDoItem>(ToDoItems.Reverse());
            }

            await _repository.SetToDoItemsAsync(ToDoItems);
            await _repository.SetDoneItemsAsync(DoneItems);
            var settings = await _repository.GetSettingsAsync();
            settings.InvertedToDoList = ToDoListInverted;
            await _repository.SetSettingsAsync(settings);

            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> LoadAsync()
        {
            var toDoItems = await _repository.GetToDoItemsAsync();
            var doneItems = await _repository.GetDoneItemsAsync();
            var settings = await _repository.GetSettingsAsync();


            ToDoItems = new ObservableCollection<ToDoItem>(toDoItems);
            DoneItems = new ObservableCollection<DoneItem>(doneItems);
            Settings = settings;

            return toDoItems != null && doneItems != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}