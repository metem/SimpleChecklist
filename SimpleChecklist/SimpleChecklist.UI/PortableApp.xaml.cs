using SimpleChecklist.UI.Commands;
using SimpleChecklist.UI.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleChecklist.UI
{
    public partial class PortableApp : Application
    {
        private readonly LoadApplicationDataCommand _loadApplicationDataCommand;

        public PortableApp(MainView mainView, LoadApplicationDataCommand loadApplicationDataCommand)
        {
            InitializeComponent();

            _loadApplicationDataCommand = loadApplicationDataCommand;
            MainPage = mainView;
        }

        protected override void OnStart()
        {
            base.OnStart();

            Task.Run(_loadApplicationDataCommand.ExecuteAsync).Wait();
        }
    }
}