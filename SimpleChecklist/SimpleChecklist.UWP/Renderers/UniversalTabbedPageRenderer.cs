using SimpleChecklist.Universal.Renderers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(UniversalTabbedPageRenderer))]

namespace SimpleChecklist.Universal.Renderers
{
    public class UniversalTabbedPageRenderer : TabbedPageRenderer
    {
        public UniversalTabbedPageRenderer()
        {
            ElementChanged += OnElementChanged;

            var inputPane = InputPane.GetForCurrentView();

            inputPane.Showing += InputPaneOnShowing;
            inputPane.Hiding += InputPaneOnHiding;
        }

        private void InputPaneOnHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            Control.Padding = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
        }

        private void InputPaneOnShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            args.EnsuredFocusedElementInView = true;
            Control.Padding = new Windows.UI.Xaml.Thickness(0, 0, 0, sender.OccludedRect.Height);
        }

        private void OnElementChanged(object sender, VisualElementChangedEventArgs visualElementChangedEventArgs)
        {
            Control.Background =
                Windows.UI.Xaml.Application.Current.Resources["MainTabbedPageBackground"] as LinearGradientBrush;

            Control.HeaderTemplate =
                Windows.UI.Xaml.Application.Current.Resources["MainTabbedPageHeaderTemplate"] as
                    Windows.UI.Xaml.DataTemplate;
        }
    }
}