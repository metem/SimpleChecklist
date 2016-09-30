using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Models.Workspaces
{
    public class TaskMainPreviewWorkspace : IWorkspace
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly Func<string, IFile> _fileFunc;

        public TaskMainPreviewWorkspace(Func<string, IFile> fileFunc, IDialogUtils dialogUtils,
            TaskListObservableCollection taskList)
        {
            _fileFunc = fileFunc;
            _dialogUtils = dialogUtils;
            TaskListObservableCollection = taskList;
        }

        public TaskListObservableCollection TaskListObservableCollection { get; }

        public async Task<bool> SaveCurrentStateAsync()
        {
            var data = XmlBinarySerializer.Serialize(TaskListObservableCollection.ToDoItems);

            try
            {
                var file = _fileFunc(AppSettings.TaskListFileName);

                if (!file.Exist)
                    await file.CreateAsync();

                await file.SaveBytesAsync(data);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, ex.Message, AppTexts.Close);
                return false;
            }

            return true;
        }

        public async Task<bool> LoadCurrentStateAsync()
        {
            byte[] data;

            try
            {
                var file = _fileFunc(AppSettings.TaskListFileName);

                if (!file.Exist)
                    return true;

                data = await file.ReadBytesAsync();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await _dialogUtils.DisplayAlertAsync(AppTexts.Error, ex.Message, AppTexts.Close);
                return false;
            }

            if (data != null)
            {
                try
                {
                    var result = XmlBinarySerializer.Deserialize<ObservableCollection<ToDoItem>>(data);
                    TaskListObservableCollection.Load(result);
                    return true;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return false;
        }

        public async Task CreateBackup()
        {
            await
                _fileFunc(AppSettings.TaskListFileName)
                    .CopyFileAsync(_fileFunc(AppSettings.TaskListFileName + AppSettings.PartialBackupFileExtension));
        }

        public async Task RestoreBackup()
        {
            await
                _fileFunc(AppSettings.TaskListFileName + AppSettings.PartialBackupFileExtension)
                    .CopyFileAsync(_fileFunc(AppSettings.TaskListFileName));
        }
    }
}