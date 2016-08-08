using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class MainView : NavigationPage
    {
        public MainView(TabbedView tabbedView)
        {
            InitializeComponent();

            SetHasNavigationBar(tabbedView, false);
        }
    }
}