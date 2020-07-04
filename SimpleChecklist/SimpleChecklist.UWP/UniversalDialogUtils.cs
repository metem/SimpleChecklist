using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.UI.Utils;
using SimpleChecklist.UI.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace SimpleChecklist.Universal
{
    public class UniversalDialogUtils : DialogUtils
    {
        public UniversalDialogUtils(Lazy<MainView> mainPage) : base(mainPage)
        {
        }

        public override async Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes)
        {
            var filePicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
                ViewMode = PickerViewMode.List
            };

            foreach (var allowedFileType in allowedFileTypes)
            {
                filePicker.FileTypeFilter.Add(allowedFileType);
            }

            var storageFile = await filePicker.PickSingleFileAsync();
            return new UniversalFile(storageFile);
        }

        public override async Task<IFile> SaveFileDialogAsync(string defaultFileName,
            IEnumerable<string> allowedFileTypes)
        {
            var filePicker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            foreach (var allowedFileType in allowedFileTypes)
            {
                filePicker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>(allowedFileType,
                    new List<string> { allowedFileType }));
            }

            filePicker.SuggestedFileName = defaultFileName;

            var storageFile = await filePicker.PickSaveFileAsync();
            return new UniversalFile(storageFile);
        }
    }
}