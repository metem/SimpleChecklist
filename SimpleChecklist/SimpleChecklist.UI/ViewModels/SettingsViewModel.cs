using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Core.Messages;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly MessagesStream _messagesStream;
        private bool _addTasksFromTextFileButtonIsEnabled;
        private bool _createBackupButtonIsEnabled;
        private bool _loadBackupButtonIsEnabled;

        public SettingsViewModel(MessagesStream messagesStream)
        {
            _messagesStream = messagesStream;
            AddTasksFromTextFileButtonIsEnabled = true;
            LoadBackupButtonIsEnabled = true;
            CreateBackupButtonIsEnabled = true;
        }

        public bool AddTasksFromTextFileButtonIsEnabled
        {
            get => _addTasksFromTextFileButtonIsEnabled;
            set
            {
                if (value == _addTasksFromTextFileButtonIsEnabled) return;
                _addTasksFromTextFileButtonIsEnabled = value;
                NotifyOfPropertyChange(() => AddTasksFromTextFileButtonIsEnabled);
            }
        }

        public bool CreateBackupButtonIsEnabled
        {
            get => _createBackupButtonIsEnabled;
            set
            {
                if (value == _createBackupButtonIsEnabled) return;
                _createBackupButtonIsEnabled = value;
                NotifyOfPropertyChange(() => CreateBackupButtonIsEnabled);
            }
        }

        public bool LoadBackupButtonIsEnabled
        {
            get => _loadBackupButtonIsEnabled;
            set
            {
                if (value == _loadBackupButtonIsEnabled) return;
                _loadBackupButtonIsEnabled = value;
                NotifyOfPropertyChange(() => LoadBackupButtonIsEnabled);
            }
        }

        public ICommand LoadBackupClickCommand => new Command(() =>
        {
            if (LoadBackupButtonIsEnabled)
            {
                LoadBackupButtonIsEnabled = false;
                _messagesStream.PutToStream(new EventMessage(EventType.LoadBackup));
                LoadBackupButtonIsEnabled = true;
            }
        });

        public ICommand CreateBackupClickCommand => new Command(() =>
        {
            if (CreateBackupButtonIsEnabled)
            {
                CreateBackupButtonIsEnabled = false;
                _messagesStream.PutToStream(new EventMessage(EventType.CreateBackup));
                CreateBackupButtonIsEnabled = true;
            }
        });

        public ICommand AddTasksFromTextFileClickCommand => new Command(() =>
        {
            if (AddTasksFromTextFileButtonIsEnabled)
            {
                AddTasksFromTextFileButtonIsEnabled = false;
                _messagesStream.PutToStream(new EventMessage(EventType.AddTasksFromTextFile));
                AddTasksFromTextFileButtonIsEnabled = true;
            }
        });
    }
}