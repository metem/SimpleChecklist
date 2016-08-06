using Caliburn.Micro.Xamarin.Forms;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Models.Workspaces;
using SimpleChecklist.Views;

namespace SimpleChecklist
{
    public class PortableApp : FormsApplication
    {
        private readonly IDialogUtils _dialogUtils;
        private readonly IAppUtils _appUtils;
        private readonly WorkspacesManager _workspacesManager;

        public PortableApp(WorkspacesManager workspacesManager, MainPage mainPage, IDialogUtils dialogUtils,
            IAppUtils appUtils)
        {
            _workspacesManager = workspacesManager;
            _dialogUtils = dialogUtils;
            _appUtils = appUtils;
            MainPage = mainPage;

            Initialize();
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