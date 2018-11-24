using SimpleChecklist.Core.Workflow;

namespace SimpleChecklist.Core.Messages
{
    internal class WorkflowFinishedMessage : IMessage
    {
        public IWorkflow Workflow { get; }

        public bool Result { get; }

        public WorkflowFinishedMessage(IWorkflow workflow, bool result)
        {
            Workflow = workflow;
            Result = result;
        }
    }
}