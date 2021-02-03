using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class TaskListViewModel : BaseViewModel
    {
        private static readonly string[] Colors = {
            "#ffffff",
            "#ff5a5a",
            "#ffff5a",
            "#5aff5a",
            "#00ffff",
            "#ea7b48",
            "#fac320",
            "#a8e221"
        };

        private readonly IDialogUtils _dialogUtils;

        private readonly Workspace _workspace;

        private bool _editing;

        private ToDoItem _editingItem;

        private string _entryText;

        private bool _invertedTodoList;

        private ObservableCollection<ToDoItem> _toDoItems = new ObservableCollection<ToDoItem>();

        public TaskListViewModel(IDialogUtils dialogUtils, Workspace workspace)
        {
            _dialogUtils = dialogUtils;
            _workspace = workspace;
            _workspace.AddToDoItem += AddToDoItem;
        }

        public ICommand AddClickCommand => new Command(() =>
        {
            if (string.IsNullOrEmpty(EntryText)) return;
            if (Editing)
            {
                _editingItem.Data = EntryText;
                Editing = false;
            }
            else
            {
                var newItem = new ToDoItem { Data = EntryText };
                AddToDoItem(newItem);
            }

            EntryText = string.Empty;
        });

        public ICommand ChangeColorClickCommand => new Command(item =>
        {
            var toDoItem = (ToDoItem)item;
            var nextColorIndex = Array.IndexOf(Colors, toDoItem.Color) + 1;
            if (nextColorIndex >= Colors.Length) nextColorIndex = 0;
            toDoItem.Color = Colors[nextColorIndex];
        });

        public ICommand DoneClickCommand => new Command(item =>
        {
            ToDoItem toDoItem = (ToDoItem)item;
            ToDoItems.Remove(toDoItem);
            _workspace.AddDoneItem(new DoneItem(toDoItem));
        });

        public ICommand EditClickCommand => new Command(item =>
        {
            Editing = true;
            _editingItem = (ToDoItem)item;
            EntryText = _editingItem.Data;
        });

        public bool Editing
        {
            get => _editing;
            private set
            {
                if (value != _editing)
                {
                    _editing = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EntryText
        {
            get => _entryText;
            set
            {
                if (value != _entryText)
                {
                    _entryText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RemoveClickCommand => new Command(async item => await RemoveToDoItemAsync((ToDoItem)item));

        public ObservableCollection<ToDoItem> ToDoItems
        {
            get => _toDoItems;
            set
            {
                if (value != _toDoItems)
                {
                    _toDoItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task RemoveToDoItemAsync(ToDoItem item)
        {
            var accepted = await _dialogUtils.DisplayAlertAsync(
                                AppTexts.Alert,
                                AppTexts.RemoveTaskConfirmationText,
                                AppTexts.Yes,
                                AppTexts.No);

            if (accepted)
            {
                ToDoItems.Remove(item);
            }
        }

        internal void InvertedToDoList(bool invertedTodoList)
        {
            if (_invertedTodoList != invertedTodoList)
            {
                _invertedTodoList = invertedTodoList;
                ToDoItems = new ObservableCollection<ToDoItem>(ToDoItems.Reverse());
            }
        }

        private void AddToDoItem(ToDoItem newItem)
        {
            if (_invertedTodoList)
            {
                ToDoItems.Insert(0, newItem);
            }
            else
            {
                ToDoItems.Add(newItem);
            }
        }
    }
}