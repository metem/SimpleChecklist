using System;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Common.Interfaces
{
    public interface IToDoItem
    {
        DateTime CreationDateTime { get; }

        string Description { get; }

        PortableColor ItemColor { get; set; }
    }
}