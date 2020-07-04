using Android.Content;
using SimpleChecklist.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(DroidEntryRenderer))]

namespace SimpleChecklist.Droid.Renderers
{
    public class DroidEntryRenderer : EntryRenderer
    {
        public DroidEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                e.NewElement.BackgroundColor = Color.FromRgba(0, 0, 0, 80);
            }
        }
    }
}