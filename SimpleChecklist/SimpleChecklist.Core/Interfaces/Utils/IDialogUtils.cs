using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChecklist.Core.Interfaces.Utils
{
    public interface IDialogUtils
    {
        Task DisplayAlertAsync(string title, string message, string cancel);

        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

        Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes);

        Task<IFile> SaveFileDialogAsync(string defaultFileName, IEnumerable<string> allowedFileTypes);
    }
}