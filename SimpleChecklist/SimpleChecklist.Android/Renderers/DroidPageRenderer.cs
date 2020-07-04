using Android.Content;
using SimpleChecklist.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentPage), typeof(DroidPageRenderer))]

namespace SimpleChecklist.Droid.Renderers
{
    public class DroidPageRenderer : PageRenderer
    {
        public DroidPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                e.NewElement.Padding = new Thickness(10, 0, 10, 0);
        }
    }
}