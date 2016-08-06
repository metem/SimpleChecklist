﻿using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Caliburn.Micro;
using SimpleChecklist.Models.Workspaces;
using Xamarin.Forms.Platform.Android;

namespace SimpleChecklist.Droid
{
    [Activity(Label = "SimpleChecklist", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(IoC.Get<PortableApp>());
        }

        protected override void OnStop()
        {
            base.OnStop();

            Task.Run(async () => await IoC.Get<WorkspacesManager>().SaveWorkspacesStateAsync()).Wait();
        }
    }
}
