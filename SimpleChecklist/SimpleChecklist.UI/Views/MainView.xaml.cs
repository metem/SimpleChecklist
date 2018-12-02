using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleChecklist.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : NavigationPage
    {
        public MainView(TabbedView tabbedView)
        {
            InitializeComponent();

            SetHasNavigationBar(tabbedView, false);
        }
    }
}