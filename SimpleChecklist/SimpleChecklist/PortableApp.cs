using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleChecklist.Models.Utils;
using SimpleChecklist.Models.Workspaces;
using SimpleChecklist.Views;
using Xamarin.Forms;

namespace SimpleChecklist
{
    public class PortableApp : Application
    {
        private readonly IList<IBaseWorkspace> _workspaces;
        private readonly IDialogUtils _dialogUtils;
        private readonly IAppUtils _appUtils;
        private bool _workspacesLoaded;

        public PortableApp(MainPage mainPage, IList<IBaseWorkspace> workspaces, IDialogUtils dialogUtils, IAppUtils appUtils)
        {
            _workspaces = workspaces;
            _dialogUtils = dialogUtils;
            _appUtils = appUtils;
            // The root page of your application
            MainPage = mainPage;
        }

        public async Task SaveWorkspacesStateAsync()
        {
            if (_workspacesLoaded)
            {
                foreach (var workspace in _workspaces)
                {
                    await workspace.SaveCurrentStateAsync();
                }
            }
        }

        protected override async void OnStart()
        {
            if (_workspacesLoaded) return;
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
                _workspacesLoaded = true;
                base.OnStart();
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task LoadWorkspacesStateAsync()
        {
            bool loadingResult = true;

            try
            {
                foreach (var workspace in _workspaces)
                {
                    var result = await workspace.LoadCurrentStateAsync();
                    if (!result)
                    {
                        loadingResult = false;
                    }
                }
            }
            catch (Exception)
            {
                loadingResult = false;
            }

            _workspacesLoaded = loadingResult;
        }
    }
}
