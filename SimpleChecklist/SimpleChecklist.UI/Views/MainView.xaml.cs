using System.ComponentModel;

namespace SimpleChecklist.UI.Views
{
    [DesignTimeVisible(false)]
    public partial class MainView : Xamarin.Forms.NavigationPage
    {
        public MainView(TabbedView tabbedView) : base(tabbedView)
        {
            InitializeComponent();

            SetHasNavigationBar(tabbedView, false);
        }
    }
}