using System;
using System.Runtime.Serialization;

namespace SimpleChecklist.Models.Collections
{
    [DataContract]
    public class ToDoItem
    {
        public ToDoItem()
        {
            CreationDateTime = DateTime.Now;
            TaskListColor = new TaskListColor();
        }

        [DataMember]
        public DateTime CreationDateTime { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public TaskListColor TaskListColor { get; set; }
    }
}