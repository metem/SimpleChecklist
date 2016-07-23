using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;

namespace SimpleChecklist.Models.Workspaces
{
    public class TaskMainPreviewWorkspace : BaseWorkspace
    {
        private readonly Func<string, IFile> _fileFunc;
        private readonly IDialogUtils _dialogUtils;
        private readonly TaskListObservableCollection _taskListObservableCollection;

        public TaskListObservableCollection TaskListObservableCollection => _taskListObservableCollection;

        public TaskMainPreviewWorkspace(Func<string, IFile> fileFunc, IDialogUtils dialogUtils,
            TaskListObservableCollection taskList) : base(ViewsId.TaskList)
        {
            _fileFunc = fileFunc;
            _dialogUtils = dialogUtils;
            _taskListObservableCollection = taskList;
        }

        public override async Task<bool> SaveCurrentStateAsync()
        {
            var data = XmlBinarySerializer.Serialize(_taskListObservableCollection.ToDoItems);

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

        public sealed override async Task<bool> LoadCurrentStateAsync()
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
                    _taskListObservableCollection.Load(result);
                    return true;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return false;
        }

        public override async Task CreateBackup()
        {
            await
                _fileFunc(AppSettings.TaskListFileName)
                    .CopyFileAsync(_fileFunc(AppSettings.TaskListFileName + AppSettings.PartialBackupFileExtension));
        }

        public override async Task RestoreBackup()
        {
            await
                _fileFunc(AppSettings.TaskListFileName + AppSettings.PartialBackupFileExtension)
                    .CopyFileAsync(_fileFunc(AppSettings.TaskListFileName));
        }
    }
}