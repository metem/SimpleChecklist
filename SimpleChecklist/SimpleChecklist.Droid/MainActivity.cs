using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SimpleChecklist.Droid
{
    [Activity(Label = "SimpleChecklist", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        private App _app;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            _app = BootstrapperDroid.Configure();

            LoadApplication(_app.GetPortableApp());

            _app.Load();
        }

        protected override void OnStop()
        {
            base.OnStop();

            _app.Save();
        }
    }
}

