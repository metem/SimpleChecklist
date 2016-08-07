using System;
using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class TaskListView : ContentPage
    {
        private readonly TaskListViewModel _taskListViewModel;

        public TaskListView(TaskListViewModel taskListViewModel)
        {
            InitializeComponent();
            BindingContext = _taskListViewModel = taskListViewModel;
        }

        private void SaveEntryOnCompleted(object sender, EventArgs e)
        {
            _taskListViewModel.AddClickCommand.Execute(e);
        }
    }
}