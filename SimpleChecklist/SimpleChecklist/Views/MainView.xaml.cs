using Xamarin.Forms;

namespace SimpleChecklist.UI.Views
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