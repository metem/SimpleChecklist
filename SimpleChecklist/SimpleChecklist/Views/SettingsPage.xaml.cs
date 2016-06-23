using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsPageViewModel settingsPageViewModel)
        {
            InitializeComponent();

            BindingContext = settingsPageViewModel;
        }
    }
}