using System;

namespace SimpleChecklist.Core.Workflow
{
    internal interface IWorkflow : IDisposable
    {
        WorkflowIds NextForSucceess { get; set; }

        WorkflowIds NextForFailure { get; set; }

        WorkflowIds WorkflowId { get; }

        void Initialize();
    }
}
