using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SimpleChecklist.Properties;

namespace SimpleChecklist.Models.Collections
{
    [DataContract]
    [KnownType(typeof(DoneItem))]
    public class ToDoItem : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}