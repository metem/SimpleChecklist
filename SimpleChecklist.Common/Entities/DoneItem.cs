using System;
using System.Runtime.Serialization;

namespace SimpleChecklist.Common.Entities
{
    [DataContract]
    public class DoneItem : ToDoItem
    {
        public DoneItem()
        {
            FinishDateTime = DateTime.Now;
        }

        public DoneItem(ToDoItem toDoItem)
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