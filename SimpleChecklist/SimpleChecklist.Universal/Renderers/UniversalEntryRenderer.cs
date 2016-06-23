using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using SimpleChecklist.Universal.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Entry), typeof(UniversalEntryRenderer))]
namespace SimpleChecklist.Universal.Renderers
{
    public class UniversalEntryRenderer : EntryRenderer
    {
        public UniversalEntryRenderer()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                if (Control != null)
                {
                    Control.Style =
                        Windows.UI.Xaml.Application.Current.Resources["DefaultEntry"] as Windows.UI.Xaml.Style;
                    
                    Control.Foreground = Windows.UI.Xaml.Application.Current.Resources["DefaultEntryForeground"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot set properties. Error: ", ex.Message);
            }
        }
    }
}