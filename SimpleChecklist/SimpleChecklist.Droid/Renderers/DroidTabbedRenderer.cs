using SimpleChecklist.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(DroidTabbedRenderer))]
namespace SimpleChecklist.Droid.Renderers
{
    public class DroidTabbedRenderer : TabbedRenderer
    {
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            SetBackgroundResource(Resource.Drawable.PageGradient);
        }
    }
}