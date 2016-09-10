using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;
using Xamarin.Forms;

namespace SimpleChecklist.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly DoneListObservableCollection _doneListObservableCollection;
        private readonly TaskListObservableCollection _taskListObservableCollection;
        private bool _addTasksFromTextFileButtonIsEnabled;
        private bool _createBackupButtonIsEnabled;
        private bool _loadBackupButtonIsEnabled;

        public SettingsViewModel(TaskListObservableCollection taskListObservableCollection,
            DoneListObservableCollection doneListObservableCollection, IDialogUtils dialogUtils)
        {
            _taskListObservableCollection = taskListObservableCollection;
            _doneListObservableCollection = doneListObservableCollection;
            _dialogUtils = dialogUtils;
            AddTasksFromTextFileButtonIsEnabled = true;
            LoadBackupButtonIsEnabled = true;
            CreateBackupButtonIsEnabled = true;
        }

        public bool AddTasksFromTextFileButtonIsEnabled
        {
            get { return _addTasksFromTextFileButtonIsEnabled; }
            set
            {
                if (value == _addTasksFromTextFileButtonIsEnabled) return;
                _addTasksFromTextFileButtonIsEnabled = value;
                NotifyOfPropertyChange(() => AddTasksFromTextFileButtonIsEnabled);
            }
        }

        public bool CreateBackupButtonIsEnabled
        {
            get { return _createBackupButtonIsEnabled; }
            set
            {
                if (value == _createBackupButtonIsEnabled) return;
                _createBackupButtonIsEnabled = value;
                NotifyOfPropertyChange(() => CreateBackupButtonIsEnabled);
            }
        }

        public bool LoadBackupButtonIsEnabled
        {
            get { return _loadBackupButtonIsEnabled; }
            set
            {
                if (value == _loadBackupButtonIsEnabled) return;
                _loadBackupButtonIsEnabled = value;
                NotifyOfPropertyChange(() => LoadBackupButtonIsEnabled);
            }
        }

        public ICommand LoadBackupClickCommand => new Command(async () =>
        {
            if (LoadBackupButtonIsEnabled)
            {
                LoadBackupButtonIsEnabled = false;
                await LoadBackup();
                LoadBackupButtonIsEnabled = true;
            }
        });

        public ICommand CreateBackupClickCommand => new Command(async () =>
        {
            if (CreateBackupButtonIsEnabled)
            {
                CreateBackupButtonIsEnabled = false;
                await CreateBackup();
                CreateBackupButtonIsEnabled = true;
            }
        });

        public ICommand AddTasksFromTextFileClickCommand => new Command(async () =>
        {
            if (AddTasksFromTextFileButtonIsEnabled)
            {
                AddTasksFromTextFileButtonIsEnabled = false;
                await AddTasksFromTextFile();
                AddTasksFromTextFileButtonIsEnabled = true;
            }
        });

        private async Task<bool> LoadBackup()
        {
            var file = await _dialogUtils.OpenFileDialogAsync(new[] {AppSettings.BackupFileExtension});

            if (file == null)
            {
                // Cancelled
                return false;
            }

            string text;

            try
            {
                text = file.Exist ? await file.ReadTextAsync() : string.Empty;
            }
            catch (ArgumentOutOfRangeException)
            {
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, AppTexts.BackupLoadError, AppTexts.Close);
                return false;
            }

            if (string.IsNullOrEmpty(text))
                return false;

            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Alert,
                AppTexts.LoadBackupConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                try
                {
                    var data = XmlStringSerializer.Deserialize<BackupData>(text);

                    if (data.ToDoItems != null)
                    {
                        _taskListObservableCollection.Load(data.ToDoItems);
                    }

                    if (data.DoneItems != null)
                    {
                        _doneListObservableCollection.Load(data.DoneItems);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                return true;
            }

            return false;
        }

        private async Task CreateBackup()
        {
            var backupData = new BackupData
            {
                ToDoItems = _taskListObservableCollection.ToDoItems,
                DoneItems = _doneListObservableCollection.DoneItemsGroups
            };
            var data = XmlStringSerializer.Serialize(backupData);

            var file =
                await
                    _dialogUtils.SaveFileDialogAsync(
                        $"{AppSettings.BackupFileName}-{DateTime.Now:yy.MM.dd_HH_mm_ss}",
                        new[] {AppSettings.BackupFileExtension});

            if (file == null)
            {
                // Cancelled
                return;
            }

            try
            {
                await file.SaveTextAsync(data);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, ex.Message, AppTexts.Close);
            }
        }

        private async Task AddTasksFromTextFile()
        {
            var file = await _dialogUtils.OpenFileDialogAsync(new[] {AppSettings.TextFileExtension});

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
                text = text.Replace("\t", string.Empty).Replace("\r", string.Empty);
                var tasks = text.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                var tasksReversed = tasks.Reverse();
                foreach (var task in tasksReversed)
                {
                    _taskListObservableCollection.Add(task);
                }
            }
        }
    }
}