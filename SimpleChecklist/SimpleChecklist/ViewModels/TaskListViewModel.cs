using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;
using Xamarin.Forms;

namespace SimpleChecklist.ViewModels
{
    public class TaskListViewModel : Screen
    {
        private readonly DoneListObservableCollection _doneList;
        private readonly IDialogUtils _dialogUtils;
        private string _entryText;

        public TaskListObservableCollection TaskListObservableCollection { get; }

        public TaskListViewModel(TaskListObservableCollection taskList, DoneListObservableCollection doneList, IDialogUtils dialogUtils)
        {
            TaskListObservableCollection = taskList;
            _doneList = doneList;
            _dialogUtils = dialogUtils;
        }

        public ICommand AddClickCommand => new Command(() =>
        {
            if (!string.IsNullOrEmpty(EntryText))
            {
                TaskListObservableCollection.Add(EntryText);
                EntryText = string.Empty;
            }
        });

        public ICommand RemoveClickCommand => new Command(async item =>
        {
            var accepted =
                await
                    _dialogUtils.DisplayAlertAsync(
                        AppTexts.Alert,
                        AppTexts.RemoveTaskConfirmationText,
                        AppTexts.Yes,
                        AppTexts.No);

            if (accepted)
            {
                TaskListObservableCollection.ToDoItems.Remove((ToDoItem) item);
            }
        });

        public ICommand DoneClickCommand => new Command(item =>
        {
            var toDoItem = (ToDoItem) item;

            _doneList.Add(toDoItem);
            TaskListObservableCollection.ToDoItems.Remove(toDoItem);
        });

        public ICommand ChangeColorClickCommand => new Command(item =>
        {
            var toDoItem = (ToDoItem) item;

            toDoItem.TaskListColor.MoveToNext();
        });

        public string EntryText
        {
            get { return _entryText; }
            set
            {
                _entryText = value;
                NotifyOfPropertyChange(() => EntryText);
            }
        }
    }
}
