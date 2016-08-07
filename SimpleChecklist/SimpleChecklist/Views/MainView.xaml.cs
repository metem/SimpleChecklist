using Autofac.Features.Indexed;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class MainView : NavigationPage
    {
        public MainView(IIndex<ViewsId, ContentPage> pages)
        {
            InitializeComponent();

            PushAsync(MainTabbedPage);

            MainTabbedPage.Children.Add(pages[ViewsId.TaskList]);
            MainTabbedPage.Children.Add(pages[ViewsId.DoneList]);
            MainTabbedPage.Children.Add(pages[ViewsId.Settings]);
            MainTabbedPage.Children.Add(pages[ViewsId.About]);

            SetHasNavigationBar(MainTabbedPage, false);
        }
    }
}