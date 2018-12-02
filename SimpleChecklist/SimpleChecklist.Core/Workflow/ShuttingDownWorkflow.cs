using System;
using System.Reactive.Linq;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Workflow
{
    class ShuttingDownWorkflow : IWorkflow
    {
        private readonly MessagesStream _messagesStream;
        private IDisposable _subscription;

        public ShuttingDownWorkflow(MessagesStream messagesStream)
        {
            _messagesStream = messagesStream;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public WorkflowIds NextForSucceess { get; set; }
        public WorkflowIds NextForFailure { get; set; }
        public WorkflowIds WorkflowId => WorkflowIds.ShutdownStarted;

        public void Initialize()
        {
            var stream = _messagesStream.GetStream();
            _subscription = stream.OfType<EventMessage>().Subscribe(OnNext);
        }

        private void OnNext(EventMessage message)
        {
        }
    }
}