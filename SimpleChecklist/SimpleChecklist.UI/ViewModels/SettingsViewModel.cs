using Newtonsoft.Json;
using SimpleChecklist.Common;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core;
using SimpleChecklist.Core.DTOs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDialogUtils _dialogUtils;
        private readonly DoneListViewModel _doneListViewModel;
        private readonly TaskListViewModel _taskListViewModel;
        private bool _addTasksFromTextFileButtonIsEnabled;
        private bool _createBackupButtonIsEnabled;
        private bool _invertedTodoList;
        private bool _loadBackupButtonIsEnabled;
        private bool _saveTasksToTextFileButtonIsEnabled;

        public SettingsViewModel(
            IDialogUtils dialogUtils,
            TaskListViewModel taskListViewModel,
            DoneListViewModel doneListViewModel,
            IDateTimeProvider dateTimeProvider)
        {
            AddTasksFromTextFileButtonIsEnabled = true;
            SaveTasksToTextFileButtonIsEnabled = true;
            LoadBackupButtonIsEnabled = true;
            CreateBackupButtonIsEnabled = true;
            _dialogUtils = dialogUtils;
            _taskListViewModel = taskListViewModel;
            _doneListViewModel = doneListViewModel;
            _dateTimeProvider = dateTimeProvider;
        }

        public bool AddTasksFromTextFileButtonIsEnabled
        {
            get => _addTasksFromTextFileButtonIsEnabled;
            set
            {
                if (value != _addTasksFromTextFileButtonIsEnabled)
                {
                    _addTasksFromTextFileButtonIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddTasksFromTextFileClickCommand => new Command(async () => await AddTasksFromTextFileAsync());

        public bool CreateBackupButtonIsEnabled
        {
            get => _createBackupButtonIsEnabled;
            set
            {
                if (value != _createBackupButtonIsEnabled)
                {
                    _createBackupButtonIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CreateBackupClickCommand => new Command(async () => await CreateBackupAsync());

        public bool InvertedToDoList
        {
            get { return _invertedTodoList; }
            set
            {
                if (_invertedTodoList != value)
                {
                    _invertedTodoList = value;
                    OnPropertyChanged();
                    _taskListViewModel.InvertedToDoList(_invertedTodoList);
                }
            }
        }

        public bool LoadBackupButtonIsEnabled
        {
            get => _loadBackupButtonIsEnabled;
            set
            {
                if (value != _loadBackupButtonIsEnabled)
                {
                    _loadBackupButtonIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadBackupClickCommand => new Command(async () => await LoadBackupAsync());

        public bool SaveTasksToTextFileButtonIsEnabled
        {
            get => _saveTasksToTextFileButtonIsEnabled;
            set
            {
                if (value != _saveTasksToTextFileButtonIsEnabled)
                {
                    _saveTasksToTextFileButtonIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveTasksToTextFileClickCommand => new Command(async () => await SaveTasksToTextFileAsync());

        public async Task AddTasksFromTextFileAsync()
        {
            if (AddTasksFromTextFileButtonIsEnabled)
            {
                AddTasksFromTextFileButtonIsEnabled = false;
                try
                {
                    var file = await _dialogUtils.OpenFileDialogAsync(new[] { AppSettings.TextFileExtension });

                    if (file == null)
                    {
                        // Cancelled
                        return;
                    }

                    string text;

                    try
                    {
                        text = file.Exist ? await file.ReadTextAsync() : string.Empty;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        await _dialogUtils.DisplayAlertAsync(AppTexts.Error, AppTexts.TextFileLoadError, AppTexts.Close);
                        return;
                    }

                    if (string.IsNullOrEmpty(text))
                        return;

                    var accepted = await _dialogUtils.DisplayAlertAsync(
                        AppTexts.Alert,
                        AppTexts.LoadTasksFromTextFileConfirmationText,
                        AppTexts.Yes,
                        AppTexts.No);

                    if (accepted)
                    {
                        if (_taskListViewModel.ToDoItems == null)
                        {
                            _taskListViewModel.ToDoItems = new ObservableCollection<ToDoItem>();
                        }

                        text = text.Replace("\t", string.Empty).Replace("\r", string.Empty);
                        var tasks = text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        var tasksReversed = tasks.Reverse();
                        foreach (var task in tasksReversed)
                        {
                            _taskListViewModel.ToDoItems.Add(new ToDoItem { Data = task });
                        }
                    }
                }
                finally
                {
                    AddTasksFromTextFileButtonIsEnabled = true;
                }
            }
        }

        public async Task CreateBackupAsync()
        {
            if (CreateBackupButtonIsEnabled)
            {
                CreateBackupButtonIsEnabled = false;
                try
                {
                    var file = await _dialogUtils.SaveFileDialogAsync($"{AppSettings.ManualBackupFileName}-{_dateTimeProvider.Now:yy.MM.dd_HH_mm_ss}", new[] { AppSettings.ManualBackupFileExtension });

                    if (file == null || string.IsNullOrEmpty(file.NameWithExtension))
                    {
                        // Cancelled
                        return;
                    }

                    var fileData = new FileData
                    {
                        ToDoItems = _taskListViewModel.ToDoItems,
                        DoneItems = _doneListViewModel.DoneItems
                    };

                    var serializedData = JsonConvert.SerializeObject(fileData);

                    if (!file.Exist) await file.CreateAsync();
                    await file.SaveTextAsync(serializedData);
                }
                finally
                {
                    CreateBackupButtonIsEnabled = true;
                }
            }
        }

        public async Task LoadBackupAsync()
        {
            if (LoadBackupButtonIsEnabled)
            {
                LoadBackupButtonIsEnabled = false;
                try
                {
                    var file = await _dialogUtils.OpenFileDialogAsync(new[] { AppSettings.ManualBackupFileExtension });

                    if (file == null || !file.Exist)
                    {
                        // Cancelled
                        return;
                    }

                    var accepted = await _dialogUtils.DisplayAlertAsync(
                        AppTexts.Alert,
                        AppTexts.LoadBackupConfirmationText,
                        AppTexts.Yes,
                        AppTexts.No);

                    if (accepted)
                    {
                        try
                        {
                            var serializedData = await file.ReadTextAsync();
                            var deserializedUser = JsonConvert.DeserializeObject<FileData>(serializedData);
                            _taskListViewModel.ToDoItems = new ObservableCollection<ToDoItem>(deserializedUser.ToDoItems);
                            _doneListViewModel.DoneItems = deserializedUser.DoneItems.ToList();
                        }
                        catch
                        {
                            await _dialogUtils.DisplayAlertAsync(AppTexts.Error, AppTexts.BackupLoadError, AppTexts.Close);
                        }
                    }
                }
                finally
                {
                    LoadBackupButtonIsEnabled = true;
                }
            }
        }

        public async Task SaveTasksToTextFileAsync()
        {
            if (SaveTasksToTextFileButtonIsEnabled)
            {
                SaveTasksToTextFileButtonIsEnabled = false;
                try
                {
                    var file = await _dialogUtils.SaveFileDialogAsync("list", new[] { AppSettings.TextFileExtension });

                    if (file == null)
                    {
                        // Cancelled
                        return;
                    }

                    string todolist = string.Empty;

                    if (_taskListViewModel.ToDoItems?.Any() == true)
                    {
                        todolist = _taskListViewModel.ToDoItems.Select(t => t.Data).Aggregate((t1, t2) => t1 + "\r\n" + t2);
                    }

                    string donelist = string.Empty;

                    if (_doneListViewModel.DoneItems?.Any() == true)
                    {
                        donelist = _doneListViewModel.DoneItems.Select(t => t.Data).Aggregate((t1, t2) => t1 + "\r\n" + t2);
                    }

                    await file.SaveTextAsync("To do:\r\n\r\n" + todolist + "\r\n\r\nDone:\r\n\r\n" + donelist);
                }
                finally
                {
                    SaveTasksToTextFileButtonIsEnabled = true;
                }
            }
        }
    }
}