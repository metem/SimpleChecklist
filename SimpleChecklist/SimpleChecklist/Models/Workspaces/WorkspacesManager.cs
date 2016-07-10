﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChecklist.Models.Workspaces
{
    public class WorkspacesManager
    {
        private readonly IList<IBaseWorkspace> _workspaces;

        public WorkspacesManager(IList<IBaseWorkspace> workspaces)
        {
            _workspaces = workspaces;
        }

        public bool WorkspacesLoaded { get; private set; }

        public async Task SaveWorkspacesStateAsync()
        {
            if (WorkspacesLoaded)
            {
                foreach (var workspace in _workspaces)
                {
                    await workspace.SaveCurrentStateAsync();
                }
            }
        }

        public async Task LoadWorkspacesStateAsync(bool forceLoad = false)
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

            WorkspacesLoaded = forceLoad || loadingResult;
        }

        public async Task RestoreBackup()
        {
            foreach (var workspace in _workspaces)
            {
                await workspace.RestoreBackup();
            }
        }

        public async Task CreateBackup()
        {
            foreach (var workspace in _workspaces)
            {
                await workspace.CreateBackup();
            }
        }
    }
}