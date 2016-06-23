using System;
using SimpleChecklist.ViewModels;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class TaskListPage : ContentPage
    {
        private readonly TaskListPageViewModel _taskListPageViewModel;

        public TaskListPage(TaskListPageViewModel taskListPageViewModel)
        {
            InitializeComponent();
            BindingContext = _taskListPageViewModel = taskListPageViewModel;
        }

        private void SaveEntryOnCompleted(object sender, EventArgs e)
        {
            _taskListPageViewModel.AddClickCommand.Execute(e);
        }
    }
}