using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SimpleChecklist.LegacyDataRepository.Models.Collections
{
    [DataContract]
    public class BackupData
    {
        [DataMember]
        public ObservableCollection<ToDoItem> ToDoItems { get; set; }

        [DataMember]
        public ObservableCollection<DoneItemsGroup> DoneItems { get; set; }
    }
}