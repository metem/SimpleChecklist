using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class DoneListView : ContentPage
    {
        public DoneListView(DoneListViewModel doneListViewModel)
        {
            InitializeComponent();

            BindingContext = doneListViewModel;
        }
    }
}