using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;

namespace SimpleChecklist.Universal
{
    public class UniversalDialogUtils : DialogUtils
    {
        public UniversalDialogUtils(Lazy<MainPage> mainPage) : base(mainPage)
        {
        }

        public override async Task<object> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes)
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
            return storageFile;
        }

        public override async Task<object> SaveFileDialogAsync(string defaultFileName,
            IEnumerable<string> allowedFileTypes)
        {
            var filePicker = new FileSavePicker()
            {
                SuggestedStartLocation = PickerLocationId.Downloads
            };

            foreach (var allowedFileType in allowedFileTypes)
            {
                filePicker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>(allowedFileType,
                    new List<string> {allowedFileType}));
            }

            filePicker.SuggestedFileName = defaultFileName;

            var storageFile = await filePicker.PickSaveFileAsync();
            return storageFile;
        }
    }
}