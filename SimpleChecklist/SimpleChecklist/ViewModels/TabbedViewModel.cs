using Caliburn.Micro;

namespace SimpleChecklist.ViewModels
{
    public class TabbedViewModel : Screen
    {
        public TaskListViewModel TaskListViewModel { get; set; }

        public DoneListViewModel DoneListViewModel { get; set; }

        public SettingsViewModel SettingsViewModel { get; set; }

        public AboutViewModel AboutViewModel { get; set; }

        public TabbedViewModel(TaskListViewModel taskListViewModel, DoneListViewModel doneListViewModel,
            SettingsViewModel settingsViewModel, AboutViewModel aboutViewModel)
        {
            TaskListViewModel = taskListViewModel;
            DoneListViewModel = doneListViewModel;
            SettingsViewModel = settingsViewModel;
            AboutViewModel = aboutViewModel;
        }
    }
}
