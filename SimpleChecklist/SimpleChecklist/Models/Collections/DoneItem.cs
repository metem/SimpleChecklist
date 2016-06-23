using System;
using System.Runtime.Serialization;

namespace SimpleChecklist.Models.Collections
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
            TaskListColor = toDoItem.TaskListColor;
        }

        [DataMember]
        public DateTime FinishDateTime { get; set; }

        public string FinishTime => FinishDateTime.ToString(AppSettings.DoneItemFinishTimeFormat);
    }
}