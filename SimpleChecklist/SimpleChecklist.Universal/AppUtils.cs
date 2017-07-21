using Windows.UI.Xaml;
using SimpleChecklist.Common.Interfaces.Utils;

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