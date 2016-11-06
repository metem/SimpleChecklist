using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Core.Entities;
using SimpleChecklist.Core.Interfaces;
using SimpleChecklist.Core.Messages;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class TaskListViewModel : Screen
    {
        private readonly MessagesStream _messagesStream;
        private string _entryText;

        public TaskListViewModel(IApplicationRepository applicationRepository, MessagesStream messagesStream)
        {
            _messagesStream = messagesStream;
            ApplicationRepository = applicationRepository;
        }

        public IApplicationRepository ApplicationRepository { get; }

        public ICommand RemoveClickCommand => new Command(item =>
        {
            _messagesStream.PutToStream(new ToDoItemActionMessage((IToDoItem) item,
                ToDoItemAction.Remove));
        });

        public ICommand DoneClickCommand
            =>
            new Command(
                item =>
                {
                    _messagesStream.PutToStream(new ToDoItemActionMessage((IToDoItem) item,
                        ToDoItemAction.MoveToDoneList));
                });

        public ICommand ChangeColorClickCommand
            =>
            new Command(
                item =>
                {
                    _messagesStream.PutToStream(new ToDoItemActionMessage((IToDoItem) item, ToDoItemAction.SwitchColor));
                });

        public string EntryText
        {
            get { return _entryText; }
            set
            {
                if (value == _entryText) return;
                _entryText = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddClick()
        {
            if (!string.IsNullOrEmpty(EntryText))
            {
                _messagesStream.PutToStream(new ToDoItemActionMessage(new ToDoItem {Description = EntryText},
                    ToDoItemAction.Add));
                EntryText = string.Empty;
            }
        }
    }
}