using Windows.UI.Xaml;
using SimpleChecklist.Models.Utils;

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