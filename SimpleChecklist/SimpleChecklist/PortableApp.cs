using Caliburn.Micro.Xamarin.Forms;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Models.Workspaces;
using SimpleChecklist.Views;
using Xamarin.Forms;

namespace SimpleChecklist
{
    public class PortableApp : FormsApplication
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly IAppUtils _appUtils;
        private readonly WorkspacesManager _workspacesManager;
        private readonly MainView _mainView;

        public PortableApp(WorkspacesManager workspacesManager, MainView mainView, IDialogUtils dialogUtils,
            IAppUtils appUtils)
        {
            _workspacesManager = workspacesManager;
            _mainView = mainView;
            _dialogUtils = dialogUtils;
            _appUtils = appUtils;

            Initialize();

            DisplayRootView<TabbedView>();
        }

        protected override NavigationPage CreateApplicationPage()
        {
            return _mainView;
        }

        protected override async void OnStart()
        {
            if (_workspacesManager.WorkspacesLoaded)
            {
                await _workspacesManager.CreateBackup();
                return;
            }

            var accepted = await _dialogUtils.DisplayAlertAsync(
                AppTexts.Error,
                AppTexts.LoadErrorConfirmationText,
                AppTexts.Yes,
                AppTexts.No);

            if (accepted)
            {
                _appUtils.Close();
            }
            else
            {
                await _workspacesManager.RestoreBackup();
                await _workspacesManager.LoadWorkspacesStateAsync(true);
                base.OnStart();
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}