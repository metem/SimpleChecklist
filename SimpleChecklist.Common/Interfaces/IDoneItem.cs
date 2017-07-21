using System;

namespace SimpleChecklist.Common.Interfaces
{
    public interface IDoneItem : IToDoItem
    {
        DateTime FinishDateTime { get; }
    }
}