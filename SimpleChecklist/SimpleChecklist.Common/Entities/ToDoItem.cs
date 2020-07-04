using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleChecklist.Common.Entities
{
    public class ToDoItem : INotifyPropertyChanged, IEquatable<ToDoItem>
    {
        private string _color;
        private string _data;

        public ToDoItem()
        {
            CreationDateTime = DateTime.UtcNow.RemoveMiliseconds();
            _color = "#ffffff";
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public DateTime CreationDateTime { get; set; }

        public string Data
        {
            get => _data;
            set
            {
                if (string.Equals(value, _data)) return;
                _data = value;
                OnPropertyChanged();
            }
        }

        public static bool operator !=(ToDoItem left, ToDoItem right)
        {
            return !(left == right);
        }

        public static bool operator ==(ToDoItem left, ToDoItem right)
        {
            return EqualityComparer<ToDoItem>.Default.Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ToDoItem);
        }

        public bool Equals(ToDoItem other)
        {
            return other != null &&
                   Color == other.Color &&
                   CreationDateTime == other.CreationDateTime &&
                   Data == other.Data;
        }

        public override int GetHashCode()
        {
            int hashCode = 1554446366;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + CreationDateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Data);
            return hashCode;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}