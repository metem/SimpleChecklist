using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class AboutView : ContentPage
    {
        public AboutView(AboutViewModel aboutViewModel)
        {
            InitializeComponent();

            BindingContext = aboutViewModel;
        }
    }
}