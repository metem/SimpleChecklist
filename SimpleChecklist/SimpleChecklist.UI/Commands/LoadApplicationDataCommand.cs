using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Commands;
using SimpleChecklist.Core.DTOs;
using SimpleChecklist.Core.Repositories;
using SimpleChecklist.UI.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.UI.Commands
{
    public class LoadApplicationDataCommand : ICommand
    {
        private readonly BackupRepository _backupRepository;
        private readonly DoneListViewModel _doneListViewModel;
        private readonly FileDataRepository _fileDataRepository;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly TaskListViewModel _taskListViewModel;

        public LoadApplicationDataCommand(
            TaskListViewModel taskListViewModel,
            DoneListViewModel doneListViewModel,
            SettingsViewModel settingsViewModel,
            FileDataRepository fileDataRepository,
            BackupRepository backupRepository)
        {
            _taskListViewModel = taskListViewModel;
            _doneListViewModel = doneListViewModel;
            _settingsViewModel = settingsViewModel;
            _fileDataRepository = fileDataRepository;
            _backupRepository = backupRepository;
        }

        public async Task ExecuteAsync()
        {
            bool dataLoaded = false;
            if (_fileDataRepository.ApplicationDataExists())
            {
                dataLoaded = await LoadApplicationDataAsync();
            }

            if (!dataLoaded)
            {
                if (await _backupRepository.RestoreLastBackupAsync())
                {
                    await LoadApplicationDataAsync();
                }
            }
        }

        private async Task<bool> LoadApplicationDataAsync()
        {
            try
            {
                FileData fileData = await _fileDataRepository.GetFileDataAsync();
                _taskListViewModel.ToDoItems = new ObservableCollection<ToDoItem>(fileData.ToDoItems);
                _doneListViewModel.DoneItems = fileData.DoneItems.ToList();
                _settingsViewModel.InvertedToDoList = fileData.Settings?.InvertedToDoList == true;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}