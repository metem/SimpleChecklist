using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView(SettingsViewModel settingsViewModel)
        {
            InitializeComponent();

            BindingContext = settingsViewModel;
        }
    }
}