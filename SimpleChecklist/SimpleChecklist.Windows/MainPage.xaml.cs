using SimpleChecklist.Droid;

namespace SimpleChecklist.Windows
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var app = BootstrapperWindows.Configure();

            LoadApplication(app);
        }
    }
}
