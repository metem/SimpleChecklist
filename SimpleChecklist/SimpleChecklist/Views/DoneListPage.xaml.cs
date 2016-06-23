using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class DoneListPage : ContentPage
    {
        public DoneListPage(DoneListPageViewModel doneListPageViewModel)
        {
            InitializeComponent();

            BindingContext = doneListPageViewModel;
        }
    }
}