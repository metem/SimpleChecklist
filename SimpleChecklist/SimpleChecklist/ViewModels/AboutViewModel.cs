using System;
using System.Windows.Input;
using Caliburn.Micro;
using SimpleChecklist.Core;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class AboutViewModel : Screen
    {
        public ICommand UrlClickCommand
            => new Command(() => { Device.OpenUri(new Uri($"http://{AppSettings.WebsiteUrl}")); });
    }
}