using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Models.Workspaces
{
    public class TaskMainPreviewWorkspace : BaseWorkspace
    {
        private readonly IFileUtils _fileUtils;
        private readonly IDialogUtils _dialogUtils;
        private readonly TaskListObservableCollection _taskListObservableCollection;

        public TaskListObservableCollection TaskListObservableCollection => _taskListObservableCollection;

        public TaskMainPreviewWorkspace(IFileUtils fileUtils, IDialogUtils dialogUtils, TaskListObservableCollection taskList) : base(ViewsId.TaskList)
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
                await _fileUtils.SaveBytesAsync(AppSettings.TaskListFileName, data, true);
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
                data = await _fileUtils.ReadBytesAsync(AppSettings.TaskListFileName, true);

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
    }
}