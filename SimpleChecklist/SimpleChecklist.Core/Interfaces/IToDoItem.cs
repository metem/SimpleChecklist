using System;
using SimpleChecklist.Core.Entities;

namespace SimpleChecklist.Core.Interfaces
{
    public interface IToDoItem
    {
        DateTime CreationDateTime { get; }

        string Description { get; }

        PortableColor ItemColor { get; set; }
    }
}