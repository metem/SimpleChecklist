using SimpleChecklist.UI.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class AboutView : ContentPage
    {
        public AboutView(AboutViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}