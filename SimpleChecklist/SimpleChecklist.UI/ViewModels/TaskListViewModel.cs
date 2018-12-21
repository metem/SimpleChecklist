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

        public bool Editing
        {
            get => _editing;
            private set
            {
                if (value == _editing) return;
                _editing = value;
                NotifyOfPropertyChange();
            }
        }

        private ToDoItem editingItem;
        private bool _editing;

        public TaskListViewModel(MessagesStream messagesStream, ApplicationData appData)
        {
            _messagesStream = messagesStream;
            AppData = appData;
        }

        public ICommand RemoveClickCommand => new Command(item =>
        {
            _messagesStream.PutToStream(new ToDoItemActionMessage((ToDoItem)item,
                ToDoItemAction.Remove));
        });

        public ICommand EditClickCommand => new Command(item =>
        {
            Editing = true;
            editingItem = (ToDoItem)item;
            EntryText = editingItem.Data;
        });

        public ICommand DoneClickCommand
            =>
                new Command(
                    item =>
                    {
                        _messagesStream.PutToStream(new ToDoItemActionMessage((ToDoItem)item,
                            ToDoItemAction.MoveToDoneList));
                    });

        public ICommand ChangeColorClickCommand
            =>
                new Command(
                    item =>
                    {
                        _messagesStream.PutToStream(new ToDoItemActionMessage((ToDoItem)item,
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
            if (Editing)
            {
                _messagesStream.PutToStream(new ToDoItemActionMessage(editingItem,
                    ToDoItemAction.Update, EntryText));
                Editing = false;
            }
            else
            {
                _messagesStream.PutToStream(new ToDoItemActionMessage(new ToDoItem { Data = EntryText },
                    ToDoItemAction.Add));
            }

            EntryText = string.Empty;
        }
    }
}