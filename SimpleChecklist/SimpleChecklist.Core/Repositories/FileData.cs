using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Repositories
{
    [DataContract]
    class FileData
    {
        [DataMember]
        public IEnumerable<ToDoItem> ToDoItems { get; set; }

        [DataMember]
        public IEnumerable<DoneItem> DoneItems { get; set; }
    }
}
