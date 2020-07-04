using SimpleChecklist.Common.Entities;
using System;

namespace SimpleChecklist.Core
{
    public class Workspace
    {
        public Action<DoneItem> AddDoneItem;
        public Action<ToDoItem> AddToDoItem;
    }
}