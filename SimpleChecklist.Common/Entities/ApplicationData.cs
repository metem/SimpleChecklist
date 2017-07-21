using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Common.Entities
{
    [DataContract, KnownType(typeof(ToDoItem)), KnownType(typeof(DoneItem))]
    public class ApplicationData
    {
        [DataMember]
        public ObservableCollection<IDoneItem> DoneItems { get; set; } = new ObservableCollection<IDoneItem>();

        [DataMember]
        public ObservableCollection<IToDoItem> ToDoItems { get; set; } = new ObservableCollection<IToDoItem>();
    }
}