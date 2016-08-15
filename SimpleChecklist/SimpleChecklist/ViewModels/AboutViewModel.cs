using System;
using System.Windows.Input;
using Caliburn.Micro;
using Xamarin.Forms;

namespace SimpleChecklist.ViewModels
{
    public class AboutViewModel : Screen
    {
        public ICommand UrlClickCommand
            => new Command(() => { Device.OpenUri(new Uri($"http://{AppSettings.WebsiteUrl}")); });
    }
}