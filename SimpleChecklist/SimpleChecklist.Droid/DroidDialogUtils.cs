using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Views;

namespace SimpleChecklist.Droid
{
    public class DroidDialogUtils : DialogUtils
    {
        public DroidDialogUtils(Lazy<MainPage> mainPage) : base(mainPage)
        {
        }

        public override async Task<IFile> OpenFileDialogAsync(IEnumerable<string> allowedFileTypes)
        {
            return await new Task<IFile>(() => new DroidFile(string.Empty));
        }

        public override async Task<IFile> SaveFileDialogAsync(string defaultFileName, IEnumerable<string> allowedFileTypes)
        {
            return await new Task<IFile>(() => new DroidFile(string.Empty));
        }
    }
}