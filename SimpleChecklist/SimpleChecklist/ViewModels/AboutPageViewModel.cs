using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleChecklist.ViewModels
{
    public class AboutPageViewModel
    {
        public ICommand UrlClickCommand => new Command(() =>
        {
            Device.OpenUri(new Uri($"http://{AppSettings.WebsiteUrl}"));
        });
    }
}
