using System;
using System.Runtime.Serialization;
using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Common.Entities
{
    [DataContract]
    public class DoneItem : ToDoItem, IDoneItem
    {
        public DoneItem()
        {
            FinishDateTime = DateTime.Now;
        }

        public DoneItem(IToDoItem toDoItem)
        {
            FinishDateTime = DateTime.Now;
            Description = toDoItem.Description;
            CreationDateTime = toDoItem.CreationDateTime;
            ItemColor = toDoItem.ItemColor;
        }

        [DataMember]
        public DateTime FinishDateTime { get; set; }

        public string FinishTime => FinishDateTime.ToString("HH:mm"); //TODO: configuration provider
    }
}