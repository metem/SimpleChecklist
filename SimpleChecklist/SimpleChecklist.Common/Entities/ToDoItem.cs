using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SimpleChecklist.Common.Entities
{
    public class ToDoItem : INotifyPropertyChanged
    {
        private string _color;
        private string _data;

        public ToDoItem()
        {
            var utcNow = DateTime.UtcNow;
            CreationDateTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute,
                utcNow.Second, DateTimeKind.Utc);

            _color = "#ffffff";
        }

        public DateTime CreationDateTime { get; set; }

        public string Data
        {
            get => _data;
            set
            {
                if (value.Equals(_data)) return;
                _data = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                if (value.Equals(_color)) return;
                _color = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}