using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Messages;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class TaskListViewModel : Screen
    {
        private readonly MessagesStream _messagesStream;

        public ApplicationData AppData { get; }

        private string _entryText;

        public TaskListViewModel(MessagesStream messagesStream, ApplicationData appData)
        {
            _messagesStream = messagesStream;
            AppData = appData;
        }

        public ICommand RemoveClickCommand => new Command(item =>
        {
            _messagesStream.PutToStream(new ToDoItemActionMessage((ToDoItem) item,
                ToDoItemAction.Remove));
        });

        public ICommand DoneClickCommand
            =>
                new Command(
                    item =>
                    {
                        _messagesStream.PutToStream(new ToDoItemActionMessage((ToDoItem) item,
                            ToDoItemAction.MoveToDoneList));
                    });

        public ICommand ChangeColorClickCommand
            =>
                new Command(
                    item =>
                    {
                        _messagesStream.PutToStream(new ToDoItemActionMessage((ToDoItem) item,
                            ToDoItemAction.SwitchColor));
                    });

        public string EntryText
        {
            get => _entryText;
            set
            {
                if (value == _entryText) return;
                _entryText = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddClick()
        {
            if (string.IsNullOrEmpty(EntryText)) return;
            _messagesStream.PutToStream(new ToDoItemActionMessage(new ToDoItem {Description = EntryText},
                ToDoItemAction.Add));
            EntryText = string.Empty;
        }
    }
}