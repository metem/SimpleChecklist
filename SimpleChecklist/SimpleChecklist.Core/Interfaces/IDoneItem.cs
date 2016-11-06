using System;

namespace SimpleChecklist.Core.Interfaces
{
    public interface IDoneItem : IToDoItem
    {
        DateTime FinishDateTime { get; }
    }
}