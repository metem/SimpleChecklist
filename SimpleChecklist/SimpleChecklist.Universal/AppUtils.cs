using Windows.UI.Xaml;
using SimpleChecklist.Core.Interfaces.Utils;

namespace SimpleChecklist.Universal
{
    public class AppUtils : IAppUtils
    {
        public void Close()
        {
            Application.Current.Exit();
        }
    }
}