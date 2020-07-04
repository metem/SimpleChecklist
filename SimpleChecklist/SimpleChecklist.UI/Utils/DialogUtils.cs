using SimpleChecklist.Common.Interfaces.Utils;
using SimpleChecklist.UI.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChecklist.UI.Utils
{
    public abstract class DialogUtils : IDialogUtils
    {
        protected readonly Lazy<MainView> MainPage;

        protected DialogUtils(Lazy<MainView> mainPage)
        {
            MainPage = mainPage;
        }

        public Task DisplayAlertAsync(string title, string message, string cancel)
        {
            return MainPage.Value.DisplayAlert(title, message, cancel);
        }

        public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        {
            return MainPage.Value.DisplayAlert(title, message, accept, cancel);
        }

        public abstract Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes);

        public abstract Task<IFile> SaveFileDialogAsync(string defaultFileName, IEnumerable<string> allowedFileTypes);
    }
}