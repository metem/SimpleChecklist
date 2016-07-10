using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;

namespace SimpleChecklist.Models.Workspaces
{
    public class TaskMainPreviewWorkspace : BaseWorkspace
    {
        private readonly IFileUtils _fileUtils;
        private readonly IDialogUtils _dialogUtils;
        private readonly TaskListObservableCollection _taskListObservableCollection;

        public TaskListObservableCollection TaskListObservableCollection => _taskListObservableCollection;

        public TaskMainPreviewWorkspace(IFileUtils fileUtils, IDialogUtils dialogUtils,
            TaskListObservableCollection taskList) : base(ViewsId.TaskList)
        {
            _fileUtils = fileUtils;
            _dialogUtils = dialogUtils;
            _taskListObservableCollection = taskList;
        }

        public override async Task<bool> SaveCurrentStateAsync()
        {
            var data = XmlBinarySerializer.Serialize(_taskListObservableCollection.ToDoItems);

            try
            {
                await _fileUtils.LocalSaveBytesAsync(AppSettings.TaskListFileName, data);
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
                data = await _fileUtils.LocalReadBytesAsync(AppSettings.TaskListFileName);

            }
            catch (FileNotFoundException)
            {
                return true;
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
                _fileUtils.LocalCopyFileAsync(AppSettings.TaskListFileName,
                    AppSettings.TaskListFileName + AppSettings.PartialBackupFileExtension);
        }

        public override async Task RestoreBackup()
        {
            await
                _fileUtils.LocalCopyFileAsync(AppSettings.TaskListFileName + AppSettings.PartialBackupFileExtension,
                    AppSettings.TaskListFileName);
        }
    }
}