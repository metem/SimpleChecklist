using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Core.Messages
{
    public class ToDoItemActionMessage : IMessage
    {
        public IToDoItem ToDoItem { get; }

        public ToDoItemAction Action { get; }

        public ToDoItemActionMessage(IToDoItem toDoItem, ToDoItemAction action)
        {
            ToDoItem = toDoItem;
            Action = action;
        }
    }
}