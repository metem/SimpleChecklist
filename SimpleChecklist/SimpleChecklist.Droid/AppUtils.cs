using Android.OS;
using SimpleChecklist.Core.Interfaces.Utils;

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