using SimpleChecklist.UI.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class DoneListView : ContentPage
    {
        public DoneListView(DoneListViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}