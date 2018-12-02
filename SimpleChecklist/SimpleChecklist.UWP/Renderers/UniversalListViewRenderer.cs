using Windows.UI.Xaml.Controls;
using SimpleChecklist.Universal.Renderers;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer (typeof(Xamarin.Forms.ListView), typeof(UniversalListViewRenderer))]
namespace SimpleChecklist.Universal.Renderers
{
    public class UniversalListViewRenderer : ListViewRenderer
    {
        private ListView _listView;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            _listView = Control as ListView;

            if (_listView != null)
            {
                if (e.NewElement != null)
                {
                    //_listView.IsMultiSelectCheckBoxEnabled = true;
                    _listView.SelectionMode = ListViewSelectionMode.None;
                }
            }
        }
    }
}
