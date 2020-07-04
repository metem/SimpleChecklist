using SimpleChecklist.Common.Interfaces.Utils;
using Windows.UI.Xaml;

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