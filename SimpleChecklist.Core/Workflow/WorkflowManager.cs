using System;
using System.Reactive.Linq;
using Autofac.Features.Indexed;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Workflow
{
    class WorkflowManager
    {
        private readonly IIndex<WorkflowIds, IWorkflow> _workflows;

        public WorkflowManager(MessagesStream messagesStream, IIndex<WorkflowIds, IWorkflow> workflows)
        {
            _workflows = workflows;
            PrepareWorkflows();

            _workflows[WorkflowIds.Initialization].Initialize();

            var stream = messagesStream.GetStream();
            stream.OfType<WorkflowFinishedMessage>().Subscribe(OnNext);
        }

        private void PrepareWorkflows()
        {
            var initializationWorkflow = _workflows[WorkflowIds.Initialization];
            initializationWorkflow.NextForSucceess = WorkflowIds.Main;
            initializationWorkflow.NextForFailure = WorkflowIds.InitializationFromBackup;

            var initializationFromBackupWorkflow = _workflows[WorkflowIds.InitializationFromBackup];
            initializationFromBackupWorkflow.NextForSucceess = WorkflowIds.Main;
            initializationFromBackupWorkflow.NextForFailure = WorkflowIds.Main;

            var mainWorkflow = _workflows[WorkflowIds.Main];
            mainWorkflow.NextForSucceess = WorkflowIds.ShutdownStarted;
            mainWorkflow.NextForFailure = WorkflowIds.ShutdownStarted;
        }

        private void OnNext(WorkflowFinishedMessage stateChangeRequest)
        {
            var next = stateChangeRequest.Result
                ? stateChangeRequest.Workflow.NextForSucceess
                : stateChangeRequest.Workflow.NextForFailure;

            stateChangeRequest.Workflow.Dispose();
            _workflows[next].Initialize();
        }
    }
}