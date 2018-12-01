using System.Collections.Generic;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Repositories
{
    class FileData
    {
        public IEnumerable<ToDoItem> ToDoItems { get; set; }

        public IEnumerable<DoneItem> DoneItems { get; set; }
    }
}
