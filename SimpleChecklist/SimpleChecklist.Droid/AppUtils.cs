using Android.OS;
using SimpleChecklist.Models.Utils;

namespace SimpleChecklist.Droid
{
    public class AppUtils : IAppUtils
    {
        public void Close()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}