using System;
using Newtonsoft.Json;

namespace SimpleChecklist.Common.Entities
{
    public class DoneItem : ToDoItem
    {
        public DoneItem()
        {
            var utcNow = DateTime.UtcNow;
            FinishDateTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute,
                utcNow.Second, DateTimeKind.Utc);
        }

        public DoneItem(ToDoItem toDoItem)
        {
            var utcNow = DateTime.UtcNow;
            FinishDateTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute,
                utcNow.Second, DateTimeKind.Utc);
            Data = toDoItem.Data;
            CreationDateTime = toDoItem.CreationDateTime;
            Color = toDoItem.Color;
        }

        public DateTime FinishDateTime { get; set; }

        [JsonIgnore]
        public string FinishTime => FinishDateTime.ToLocalTime().ToString("HH:mm"); //TODO: configuration provider
    }
}