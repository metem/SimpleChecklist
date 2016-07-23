using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Views;

namespace SimpleChecklist.Models.Utils
{
    public abstract class DialogUtils : IDialogUtils
    {
        private readonly Lazy<MainPage> _mainPage;

        protected DialogUtils(Lazy<MainPage> mainPage)
        {
            _mainPage = mainPage;
        }

        public Task DisplayAlertAsync(string title, string message, string cancel)
        {
            return _mainPage.Value.DisplayAlert(title, message, cancel);
        }

        public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            return _mainPage.Value.DisplayAlert(title, message, accept, cancel);
        }

        public abstract Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes);

        public abstract Task<IFile> SaveFileDialogAsync(string defaultFileName, IEnumerable<string> allowedFileTypes);
    }
}