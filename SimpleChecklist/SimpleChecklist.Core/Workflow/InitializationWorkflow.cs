using System;
using System.Reactive.Linq;
using SimpleChecklist.Core.Commands.General;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Workflow
{
    class InitializationWorkflow : IWorkflow
    {
        private readonly MessagesStream _messagesStream;
        private readonly LoadApplicationDataCommand _loadApplicationDataCommand;

        private readonly AskUserIfContinueAfterInitializationFailureCommand
            _askUserIfContinueAfterInitializationFailureCommand;

        private readonly CreateApplicationDataBackupCommand _createApplicationDataBackupCommand;
        private readonly InvertListOrderCommand _invertListOrderCommand;
        private IDisposable _subscription;

        public InitializationWorkflow(MessagesStream messagesStream,
            LoadApplicationDataCommand loadApplicationDataCommand,
            AskUserIfContinueAfterInitializationFailureCommand askUserIfContinueAfterInitializationFailureCommand,
            CreateApplicationDataBackupCommand createApplicationDataBackupCommand,
            InvertListOrderCommand invertListOrderCommand)
        {
            _messagesStream = messagesStream;
            _loadApplicationDataCommand = loadApplicationDataCommand;
            _askUserIfContinueAfterInitializationFailureCommand = askUserIfContinueAfterInitializationFailureCommand;
            _createApplicationDataBackupCommand = createApplicationDataBackupCommand;
            _invertListOrderCommand = invertListOrderCommand;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public WorkflowIds NextForSucceess { get; set; }
        public WorkflowIds NextForFailure { get; set; }
        public WorkflowIds WorkflowId => WorkflowIds.Initialization;

        public void Initialize()
        {
            var stream = _messagesStream.GetStream();
            _subscription = stream.OfType<EventMessage>().Subscribe(OnNext);
        }

        private async void OnNext(EventMessage message)
        {
            switch (message.EventType)
            {
                case EventType.Started:
                    await _loadApplicationDataCommand.ExecuteAsync();
                    break;
                case EventType.InvertListOrder:
                    await _invertListOrderCommand.ExecuteAsync();
                    break;
                case EventType.ApplicationDataLoadError:
                    _messagesStream.PutToStream(new WorkflowFinishedMessage(this, false));
                    await _askUserIfContinueAfterInitializationFailureCommand.ExecuteAsync();
                    break;
                case EventType.ApplicationDataLoadFinished:
                    _messagesStream.PutToStream(new WorkflowFinishedMessage(this, true));
                    _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
                    await _createApplicationDataBackupCommand.ExecuteAsync();
                    break;
            }
        }
    }
}