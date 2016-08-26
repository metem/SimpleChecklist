using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;
using Xamarin.Forms;

namespace SimpleChecklist.Droid
{
    public class DroidDialogUtils : DialogUtils
    {
        private readonly Func<IDirectory, OpenFilePickerView> _openFilePicker;
        private readonly Func<IDirectory, SaveFilePickerView> _saveFilePicker;

        public DroidDialogUtils(Lazy<MainView> mainPage,
            Func<IDirectory, OpenFilePickerView> openFilePicker,
            Func<IDirectory, SaveFilePickerView> saveFilePicker)
            : base(mainPage)
        {
            _openFilePicker = openFilePicker;
            _saveFilePicker = saveFilePicker;
        }

        public override async Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePickerDialog = _openFilePicker(new DroidDirectory(folderPath));

            var path = await filePickerDialog.ShowAsync(allowedFileTypes);

            return path != null ? new DroidFile(new FileInfo(path)) : null;
        }

        public override async Task<IFile> SaveFileDialogAsync(string defaultFileName,
            IEnumerable<string> allowedFileTypes)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePickerDialog = _saveFilePicker(new DroidDirectory(folderPath));

            var path = await filePickerDialog.ShowAsync(defaultFileName, allowedFileTypes.FirstOrDefault());

            return path != null ? new DroidFile(new FileInfo(path)) : null;
        }
    }
}