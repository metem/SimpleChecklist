using System.Collections.Generic;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Repositories
{
    class FileData
    {
        public Settings Settings { get; set; }

        public IEnumerable<ToDoItem> ToDoItems { get; set; }

        public IEnumerable<DoneItem> DoneItems { get; set; }
    }
}
