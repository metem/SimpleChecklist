using Caliburn.Micro.Xamarin.Forms;
using SimpleChecklist.Core.Messages;
using SimpleChecklist.UI.Views;
using Xamarin.Forms;

namespace SimpleChecklist.UI
{
    public partial class PortableApp : FormsApplication
    {
        private readonly MessagesStream _messagesStream;
        private readonly MainView _mainView;

        public PortableApp(MainView mainView, MessagesStream messagesStream)
        {
            InitializeComponent();

            _mainView = mainView;
            _messagesStream = messagesStream;

            Initialize();

            DisplayRootView<TabbedView>();
        }

        protected override NavigationPage CreateApplicationPage()
        {
            return _mainView;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _messagesStream.PutToStream(new EventMessage(EventType.Started));
        }
    }
}