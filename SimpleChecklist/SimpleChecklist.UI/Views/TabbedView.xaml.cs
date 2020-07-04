using System.ComponentModel;
using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class TabbedView : TabbedPage
    {
        public TabbedView(TaskListView taskListView, DoneListView doneListView, SettingsView settingsView, AboutView aboutView)
        {
            Children.Add(taskListView);
            Children.Add(doneListView);
            Children.Add(settingsView);
            Children.Add(aboutView);

            InitializeComponent();
        }
    }
}