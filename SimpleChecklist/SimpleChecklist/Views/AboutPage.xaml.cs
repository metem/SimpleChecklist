using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage(AboutPageViewModel aboutPageViewModel)
        {
            InitializeComponent();

            BindingContext = aboutPageViewModel;
        }
    }
}