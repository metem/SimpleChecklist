using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChecklist.Models.Utils
{
    public interface IDialogUtils
    {
        Task DisplayAlertAsync(string title, string message, string cancel);

        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

        Task<object> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes);

        Task<object> SaveFileDialogAsync(string defaultFileName, IEnumerable<string> allowedFileTypes);
    }
}