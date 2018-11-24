using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Messages
{
    public class ToDoItemActionMessage : IMessage
    {
        public ToDoItem ToDoItem { get; }

        public ToDoItemAction Action { get; }

        public ToDoItemActionMessage(ToDoItem toDoItem, ToDoItemAction action)
        {
            ToDoItem = toDoItem;
            Action = action;
        }
    }
}