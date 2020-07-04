using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SimpleChecklist.Common.Entities
{
    public class DoneItem : ToDoItem, IEquatable<DoneItem>
    {
        public DoneItem()
        {
            FinishDateTime = DateTime.UtcNow.RemoveMiliseconds();
        }

        public DoneItem(ToDoItem toDoItem) : this()
        {
            Data = toDoItem.Data;
            CreationDateTime = toDoItem.CreationDateTime;
            Color = toDoItem.Color;
        }

        public DateTime FinishDateTime { get; set; }

        [JsonIgnore]
        public string FinishTime => FinishDateTime.ToLocalTime().ToString("HH:mm");

        public static bool operator !=(DoneItem left, DoneItem right)
        {
            return !(left == right);
        }

        public static bool operator ==(DoneItem left, DoneItem right)
        {
            return EqualityComparer<DoneItem>.Default.Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DoneItem);
        }

        public bool Equals(DoneItem other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Color == other.Color &&
                   CreationDateTime == other.CreationDateTime &&
                   Data == other.Data &&
                   FinishDateTime == other.FinishDateTime;
        }

        public override int GetHashCode()
        {
            int hashCode = -1147776445;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + CreationDateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Data);
            hashCode = hashCode * -1521134295 + FinishDateTime.GetHashCode();
            return hashCode;
        }
    }
}