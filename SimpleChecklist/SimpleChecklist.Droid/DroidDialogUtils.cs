using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;
using Xamarin.Forms;

namespace SimpleChecklist.Droid
{
    public class DroidDialogUtils : DialogUtils
    {
        private readonly Func<INavigation, IDirectory, FilePickerView> _filePickerPage;

        public DroidDialogUtils(Lazy<MainView> mainPage, Func<INavigation, IDirectory, FilePickerView> filePickerPage)
            : base(mainPage)
        {
            _filePickerPage = filePickerPage;
        }

        public override async Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePickerDialog = _filePickerPage(MainPage.Value.Navigation, new DroidDirectory(folderPath));

            var path = await filePickerDialog.ShowAsync();

            return new DroidFile(new FileInfo(path));
        }

        public override async Task<IFile> SaveFileDialogAsync(string defaultFileName, IEnumerable<string> allowedFileTypes)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePickerDialog = _filePickerPage(MainPage.Value.Navigation, new DroidDirectory(folderPath));

            var path = await filePickerDialog.ShowAsync();

            return new DroidFile(new FileInfo(path));
        }
    }
}