using SimpleChecklist.Universal.Renderers;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ListView), typeof(UniversalListViewRenderer))]
namespace SimpleChecklist.Universal.Renderers
{
    public class UniversalListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (Control is ListView listView)
            {
                listView.SelectionMode = ListViewSelectionMode.None;
            }
            else if (Control is SemanticZoom semanticZoom)
            {
                semanticZoom.CanChangeViews = false;
                var listViewControl = semanticZoom.ZoomedInView as ListView;
                listViewControl.SelectionMode = ListViewSelectionMode.None;

                var gridView = semanticZoom.ZoomedOutView as GridView;
                gridView.ItemTemplate = Windows.UI.Xaml.Application.Current.Resources["GridViewItemTemplate"] as Windows.UI.Xaml.DataTemplate;
            }
        }
    }
}
