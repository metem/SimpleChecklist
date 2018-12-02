using System;
using System.Reactive.Linq;
using SimpleChecklist.Core.Commands.General;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Workflow
{
    internal class InitializationFromBackupWorkflow : IWorkflow
    {
        private readonly MessagesStream _messagesStream;
        private readonly RestoreApplicationDataBackupCommand _restoreApplicationDataBackupCommand;
        private readonly LoadApplicationDataCommand _loadApplicationDataCommand;
        private readonly CloseApplicationCommand _closeApplicationCommand;
        private IDisposable _subscription;

        public InitializationFromBackupWorkflow(MessagesStream messagesStream,
            RestoreApplicationDataBackupCommand restoreApplicationDataBackupCommand,
            LoadApplicationDataCommand loadApplicationDataCommand,
            CloseApplicationCommand closeApplicationCommand)
        {
            _messagesStream = messagesStream;
            _restoreApplicationDataBackupCommand = restoreApplicationDataBackupCommand;
            _loadApplicationDataCommand = loadApplicationDataCommand;
            _closeApplicationCommand = closeApplicationCommand;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public WorkflowIds NextForSucceess { get; set; }

        public WorkflowIds NextForFailure { get; set; }

        public WorkflowIds WorkflowId => WorkflowIds.InitializationFromBackup;

        public void Initialize()
        {
            var stream = _messagesStream.GetStream();
            _subscription = stream.OfType<EventMessage>().Subscribe(OnNext);
        }

        private async void OnNext(EventMessage message)
        {
            switch (message.EventType)
            {
                case EventType.InitializationFromBackupRequested:
                    await _restoreApplicationDataBackupCommand.ExecuteAsync();
                    await _loadApplicationDataCommand.ExecuteAsync();
                    break;
                case EventType.ApplicationDataLoadError:
                    _messagesStream.PutToStream(new WorkflowFinishedMessage(this, false));
                    break;
                case EventType.ApplicationDataLoadFinished:
                    _messagesStream.PutToStream(new EventMessage(EventType.DoneListRefreshRequested));
                    _messagesStream.PutToStream(new WorkflowFinishedMessage(this, true));
                    break;
                case EventType.InitializationCancelled:
                    await _closeApplicationCommand.ExecuteAsync();
                    break;
            }
        }
    }
}