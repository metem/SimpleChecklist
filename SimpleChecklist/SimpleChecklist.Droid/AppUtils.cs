using Android.OS;
using SimpleChecklist.Common.Interfaces.Utils;

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