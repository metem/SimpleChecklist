using SimpleChecklist.Universal.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Entry), typeof(UniversalEntryRenderer))]

namespace SimpleChecklist.Universal.Renderers
{
    public class UniversalEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Style = Windows.UI.Xaml.Application.Current.Resources["DefaultEntry"] as Windows.UI.Xaml.Style;
            }
        }
    }
}