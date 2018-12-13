using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Messages
{
    public class ToDoItemActionMessage : IMessage
    {
        public ToDoItem ToDoItem { get; }

        public ToDoItemAction Action { get; }

        public string NewData { get; set; }

        public ToDoItemActionMessage(ToDoItem toDoItem, ToDoItemAction action, string newData = null)
        {
            ToDoItem = toDoItem;
            Action = action;
            NewData = newData;
        }
    }
}