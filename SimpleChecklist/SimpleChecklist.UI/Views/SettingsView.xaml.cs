using SimpleChecklist.UI.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView(SettingsViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}