using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Commands;
using SimpleChecklist.Core.DTOs;
using SimpleChecklist.Core.Repositories;
using SimpleChecklist.UI.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleChecklist.UI.Commands
{
    public class SaveApplicationDataCommand : ICommand
    {
        private readonly BackupRepository _backupRepository;
        private readonly DoneListViewModel _doneListViewModel;
        private readonly FileDataRepository _fileDataRepository;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly TaskListViewModel _taskListViewModel;

        public SaveApplicationDataCommand(
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
            await _backupRepository.CreateBackupAsync();

            var fileData = new FileData
            {
                ToDoItems = _settingsViewModel.InvertedToDoList
                ? _taskListViewModel.ToDoItems.Reverse()
                : _taskListViewModel.ToDoItems,
                DoneItems = _doneListViewModel.DoneItems,
                Settings = new Settings
                {
                    InvertedToDoList = _settingsViewModel.InvertedToDoList
                }
            };

            await _fileDataRepository.UpdateFileDataAsync(fileData);
        }
    }
}