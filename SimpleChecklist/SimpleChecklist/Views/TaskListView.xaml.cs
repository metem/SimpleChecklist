using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class TaskListView : ContentPage
    {
        public TaskListView(TaskListViewModel taskListViewModel)
        {
            InitializeComponent();

            BindingContext = taskListViewModel;
        }
    }
}