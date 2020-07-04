using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Autofac;
using SimpleChecklist.UI;
using SimpleChecklist.UI.Commands;
using System.Threading.Tasks;

namespace SimpleChecklist.Droid
{
    [Activity(Label = "Simple Checklist", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly IContainer _container;

        public MainActivity()
        {
            _container = BootstrapperDroid.Configure();
            var droidDialogUtils = _container.Resolve<DroidDialogUtils>();

            droidDialogUtils.AskForPermissions = () =>
            {
                if (!ExternalStoragePermissionsGranted())
                {
                    string[] permission = { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };
                    RequestPermissions(permission, 1);
                    return false;
                }

                return true;
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(_container.Resolve<PortableApp>());
        }

        protected override void OnPause()
        {
            base.OnPause();
            Task.Run(_container.Resolve<SaveApplicationDataCommand>().ExecuteAsync).Wait();
        }

        private bool ExternalStoragePermissionsGranted()
        {
            string[] permission = { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };
            return CheckSelfPermission(permission[0]) == Permission.Granted && CheckSelfPermission(permission[1]) == Permission.Granted;
        }
    }
}