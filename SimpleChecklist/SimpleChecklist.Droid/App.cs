using System;
using System.Threading.Tasks;
using SimpleChecklist.Models.Workspaces;
using Xamarin.Forms;

namespace SimpleChecklist.Droid
{
    public class App
    {
        private readonly PortableApp _portableApp;
        private readonly Lazy<WorkspacesManager> _workspacesManager;

        public App(PortableApp portableApp, Lazy<WorkspacesManager> workspacesManager)
        {
            _portableApp = portableApp;
            _workspacesManager = workspacesManager;
        }

        public void Load()
        {
            Task.Run(async () => await _workspacesManager.Value.LoadWorkspacesStateAsync()).Wait();
        }

        public void Save()
        {
            Task.Run(async () => await _workspacesManager.Value.SaveWorkspacesStateAsync()).Wait();
        }

        public PortableApp GetPortableApp()
        {
            return _portableApp;
        }
    }
}