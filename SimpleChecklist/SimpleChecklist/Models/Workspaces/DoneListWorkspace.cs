using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Collections;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Models.Workspaces
{
    public class DoneListWorkspace : BaseWorkspace
    {
        private readonly IFileUtils _fileUtils;
        private readonly IDialogUtils _dialogUtils;

        public DoneListObservableCollection DoneListObservableCollection { get; }

        public DoneListWorkspace(IFileUtils fileUtils, IDialogUtils dialogUtils, DoneListObservableCollection doneList) : base(ViewsId.DoneList)
        {
            _fileUtils = fileUtils;
            _dialogUtils = dialogUtils;

            DoneListObservableCollection = doneList;
        }

        public override async Task<bool> SaveCurrentStateAsync()
        {
            var data = XmlBinarySerializer.Serialize(DoneListObservableCollection.DoneItemsGroups);

            try
            {
                await _fileUtils.SaveBytesAsync(AppSettings.DoneListFileName, data, true);
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
                data = await _fileUtils.ReadBytesAsync(AppSettings.DoneListFileName, true);
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
    }
}