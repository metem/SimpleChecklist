using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.Core.Repositories;

namespace SimpleChecklist.Core.Commands.General
{
    public class LoadBackupCommand : ICommand
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly ApplicationData _appData;
        private readonly MessagesStream _messagesStream;

        public LoadBackupCommand(IDialogUtils dialogUtils, MessagesStream messagesStream, ApplicationData appData)
        {
            _messagesStream = messagesStream;
            _dialogUtils = dialogUtils;
            _appData = appData;
        }

        public async Task ExecuteAsync()
        {
            var file = await _dialogUtils.OpenFileDialogAsync(new[] {AppSettings.BackupFileExtension});

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
                var serializedData = string.Empty;

                try
                {
                    serializedData = await file.ReadTextAsync();
                }
                catch (ArgumentOutOfRangeException)
                {
                    await _dialogUtils.DisplayAlertAsync(AppTexts.Error, AppTexts.BackupLoadError, AppTexts.Close);
                }

                var deserializedUser = JsonConvert.DeserializeObject<FileData>(serializedData);
                _appData.ToDoItems = new ObservableCollection<ToDoItem>(deserializedUser.ToDoItems);
                _appData.DoneItems = new ObservableCollection<DoneItem>(deserializedUser.DoneItems);
                _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
            }
        }
    }
}