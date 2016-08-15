using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;

namespace SimpleChecklist.Models.Workspaces
{
    public class DoneListWorkspace : BaseWorkspace
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly Func<string, IFile> _fileFunc;

        public DoneListWorkspace(Func<string, IFile> fileFunc, IDialogUtils dialogUtils,
            DoneListObservableCollection doneList)
            : base(ViewsId.DoneList)
        {
            _fileFunc = fileFunc;
            _dialogUtils = dialogUtils;

            DoneListObservableCollection = doneList;
        }

        public DoneListObservableCollection DoneListObservableCollection { get; }

        public override async Task<bool> SaveCurrentStateAsync()
        {
            var data = XmlBinarySerializer.Serialize(DoneListObservableCollection.DoneItemsGroups);

            try
            {
                var file = _fileFunc(AppSettings.DoneListFileName);

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
                var file = _fileFunc(AppSettings.DoneListFileName);

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
                    var result = XmlBinarySerializer.Deserialize<ObservableCollection<DoneItemsGroup>>(data);
                    DoneListObservableCollection.Load(result);
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
                _fileFunc(AppSettings.DoneListFileName)
                    .CopyFileAsync(_fileFunc(AppSettings.DoneListFileName + AppSettings.PartialBackupFileExtension));
        }

        public override async Task RestoreBackup()
        {
            await
                _fileFunc(AppSettings.DoneListFileName + AppSettings.PartialBackupFileExtension)
                    .CopyFileAsync(_fileFunc(AppSettings.DoneListFileName));
        }
    }
}