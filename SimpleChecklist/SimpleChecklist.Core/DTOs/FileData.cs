using SimpleChecklist.Common.Entities;
using System.Collections.Generic;

namespace SimpleChecklist.Core.DTOs
{
    public class FileData
    {
        public IEnumerable<DoneItem> DoneItems { get; set; }
        public Settings Settings { get; set; }
        public IEnumerable<ToDoItem> ToDoItems { get; set; }
    }
}