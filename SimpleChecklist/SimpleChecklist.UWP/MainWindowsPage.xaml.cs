using SimpleChecklist.UI;
using Windows.UI.ViewManagement;
using Xamarin.Forms.Platform.UWP;

namespace SimpleChecklist.Universal
{
    public sealed partial class MainWindowsPage : WindowsPage
    {
        public MainWindowsPage(PortableApp portableApp)
        {
            InitializeComponent();

            HideStatusBar();

            LoadApplication(portableApp);
        }

        private static void HideStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.HideAsync().GetResults();
            }
        }
    }
}