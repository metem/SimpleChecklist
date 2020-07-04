using SimpleChecklist.Core;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SimpleChecklist.UI.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand UrlClickCommand => new Command(async () => await Browser.OpenAsync(new Uri($"https://{AppSettings.WebsiteUrl}")));
    }
}