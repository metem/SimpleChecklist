using Android.App;
using Android.Content.PM;
using Android.OS;
using Caliburn.Micro;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.UI;
using Xamarin.Forms.Platform.Android;

namespace SimpleChecklist.Droid
{
    [Activity(Label = "Simple Checklist", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(IoC.Get<PortableApp>());
        }

        protected override void OnPause()
        {
            base.OnPause();

            IoC.Get<MessagesStream>().PutToStream(new EventMessage(EventType.Closing));
            //Task.Run(async () => await IoC.Get<WorkspacesManager>().SaveWorkspacesStateAsync()).Wait();
        }
    }
}