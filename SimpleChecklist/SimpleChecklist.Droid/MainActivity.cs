using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SimpleChecklist.Droid
{
    [Activity(Label = "SimpleChecklist", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            var app = BootstrapperDroid.Configure();

            LoadApplication(app);
        }
    }
}

