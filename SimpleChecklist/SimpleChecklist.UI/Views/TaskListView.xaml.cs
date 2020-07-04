using SimpleChecklist.UI.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class TaskListView : ContentPage
    {
        public TaskListView(TaskListViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}