using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Messages;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly MessagesStream _messagesStream;
        private readonly ApplicationData _applicationData;
        private bool _addTasksFromTextFileButtonIsEnabled;
        private bool _saveTasksToTextFileButtonIsEnabled;
        private bool _createBackupButtonIsEnabled;
        private bool _loadBackupButtonIsEnabled;
        private bool _invertedTodoList;

        public bool InvertedToDoList
        {
            get { return _invertedTodoList; }
            set
            {
                if (_invertedTodoList != value)
                {
                    _invertedTodoList = value;
                    NotifyOfPropertyChange();
                    _messagesStream.PutToStream(new EventMessage(EventType.InvertListOrder));
                }
            }
        }          
        

        public SettingsViewModel(MessagesStream messagesStream, ApplicationData applicationData)
        {
            _messagesStream = messagesStream;
            _applicationData = applicationData;
            AddTasksFromTextFileButtonIsEnabled = true;
            SaveTasksToTextFileButtonIsEnabled = true;
            LoadBackupButtonIsEnabled = true;
            CreateBackupButtonIsEnabled = true;
            applicationData.PropertyChanged += ApplicationData_PropertyChanged;            
        }

        private void ApplicationData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings))
            {
                InvertedToDoList = _applicationData.Settings.InvertedToDoList;
            }
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

        public bool SaveTasksToTextFileButtonIsEnabled
        {
            get => _saveTasksToTextFileButtonIsEnabled;
            set
            {
                if (value == _saveTasksToTextFileButtonIsEnabled) return;
                _saveTasksToTextFileButtonIsEnabled = value;
                NotifyOfPropertyChange(() => SaveTasksToTextFileButtonIsEnabled);
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

        public ICommand SaveTasksToTextFileClickCommand => new Command(() =>
        {
            if (SaveTasksToTextFileButtonIsEnabled)
            {
                SaveTasksToTextFileButtonIsEnabled = false;
                _messagesStream.PutToStream(new EventMessage(EventType.SaveTasksToTextFile));
                SaveTasksToTextFileButtonIsEnabled = true;
            }
        });
    }
}