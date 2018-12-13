using System;
using System.Reactive.Linq;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Core.Commands.DoneItemsCommands;
using SimpleChecklist.Core.Commands.General;
using SimpleChecklist.Core.Commands.ToDoItemsCommands;
using SimpleChecklist.Core.Messages;

namespace SimpleChecklist.Core.Workflow
{
    class MainWorkflow : IWorkflow
    {
        private readonly MessagesStream _messagesStream;
        private readonly SaveApplicationDataCommand _saveApplicationDataCommand;
        private readonly CreateBackupCommand _createBackupCommand;
        private readonly LoadBackupCommand _loadBackupCommand;
        private readonly AddTasksFromTextFileCommand _addTasksFromTextFileCommand;
        private readonly Func<ToDoItem, AddToDoItemCommand> _addToDoItemCommandFunc;
        private readonly Func<ToDoItem, RemoveToDoItemCommand> _removeToDoItemCommandFunc;
        private readonly Func<ToDoItem, MoveToDoneListCommand> _moveToDoneListCommandFunc;
        private readonly Func<ToDoItem, SwitchToDoItemColorCommand> _switchToDoItemColorCommandFunc;
        private readonly Func<ToDoItem, string, UpdateToDoItemCommand> _updateToDoItemColorCommandFunc;
        private readonly Func<DoneItem, RemoveDoneItemCommand> _removeDoneItemCommandFunc;
        private readonly Func<DoneItem, UndoneDoneItemCommand> _undoneDoneItemCommandFunc;
        private IDisposable _subscription;

        public MainWorkflow(MessagesStream messagesStream, SaveApplicationDataCommand saveApplicationDataCommand,
            CreateBackupCommand createBackupCommand, LoadBackupCommand loadBackupCommand,
            AddTasksFromTextFileCommand addTasksFromTextFileCommand,
            Func<ToDoItem, AddToDoItemCommand> addToDoItemCommandFunc,
            Func<ToDoItem, RemoveToDoItemCommand> removeToDoItemCommandFunc,
            Func<ToDoItem, MoveToDoneListCommand> moveToDoneListCommandFunc,
            Func<ToDoItem, SwitchToDoItemColorCommand> switchToDoItemColorCommandFunc,
            Func<ToDoItem, string, UpdateToDoItemCommand> updateToDoItemColorCommandFunc,
            Func<DoneItem, RemoveDoneItemCommand> removeDoneItemCommandFunc,
            Func<DoneItem, UndoneDoneItemCommand> undoneDoneItemCommandFunc)
        {
            _messagesStream = messagesStream;
            _saveApplicationDataCommand = saveApplicationDataCommand;
            _createBackupCommand = createBackupCommand;
            _loadBackupCommand = loadBackupCommand;
            _addTasksFromTextFileCommand = addTasksFromTextFileCommand;
            _addToDoItemCommandFunc = addToDoItemCommandFunc;
            _removeToDoItemCommandFunc = removeToDoItemCommandFunc;
            _moveToDoneListCommandFunc = moveToDoneListCommandFunc;
            _switchToDoItemColorCommandFunc = switchToDoItemColorCommandFunc;
            _updateToDoItemColorCommandFunc = updateToDoItemColorCommandFunc;
            _removeDoneItemCommandFunc = removeDoneItemCommandFunc;
            _undoneDoneItemCommandFunc = undoneDoneItemCommandFunc;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public WorkflowIds NextForSucceess { get; set; }
        public WorkflowIds NextForFailure { get; set; }
        public WorkflowIds WorkflowId => WorkflowIds.Main;

        public void Initialize()
        {
            var stream = _messagesStream.GetStream();
            _subscription = stream.OfType<EventMessage>().Subscribe(OnNext);
            _subscription = stream.OfType<ToDoItemActionMessage>().Subscribe(OnNext);
            _subscription = stream.OfType<DoneItemActionMessage>().Subscribe(OnNext);
        }

        private async void OnNext(EventMessage message)
        {
            switch (message.EventType)
            {
                case EventType.Suspending:
                    await _saveApplicationDataCommand.ExecuteAsync();
                    break;

                case EventType.Closing:
                    _messagesStream.PutToStream(new WorkflowFinishedMessage(this, true));
                    await _saveApplicationDataCommand.ExecuteAsync();
                    break;

                case EventType.CreateBackup:
                    await _createBackupCommand.ExecuteAsync();
                    break;

                case EventType.LoadBackup:
                    await _loadBackupCommand.ExecuteAsync();
                    break;

                case EventType.AddTasksFromTextFile:
                    await _addTasksFromTextFileCommand.ExecuteAsync();
                    break;
            }
        }

        private async void OnNext(ToDoItemActionMessage message)
        {
            switch (message.Action)
            {
                case ToDoItemAction.Add:
                    await _addToDoItemCommandFunc(message.ToDoItem).ExecuteAsync();
                    break;
                case ToDoItemAction.Remove:
                    await _removeToDoItemCommandFunc(message.ToDoItem).ExecuteAsync();
                    break;
                case ToDoItemAction.MoveToDoneList:
                    await _moveToDoneListCommandFunc(message.ToDoItem).ExecuteAsync();
                    break;
                case ToDoItemAction.SwitchColor:
                    await _switchToDoItemColorCommandFunc(message.ToDoItem).ExecuteAsync();
                    break;
                case ToDoItemAction.Update:
                    await _updateToDoItemColorCommandFunc(message.ToDoItem, message.NewData).ExecuteAsync();
                    break;
            }
        }

        private async void OnNext(DoneItemActionMessage message)
        {
            switch (message.Action)
            {
                case DoneItemAction.Remove:
                    await _removeDoneItemCommandFunc(message.DoneItem).ExecuteAsync();
                    break;
                case DoneItemAction.Undone:
                    await _undoneDoneItemCommandFunc(message.DoneItem).ExecuteAsync();
                    break;
            }
        }
    }
}