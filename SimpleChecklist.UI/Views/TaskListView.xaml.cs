using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleChecklist.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskListView : ContentPage
    {
        public TaskListView()
        {
            InitializeComponent();
        }
    }
}