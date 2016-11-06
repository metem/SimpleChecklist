﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SimpleChecklist.Core.Interfaces;

namespace SimpleChecklist.Core.Entities
{
    [DataContract]
    [KnownType(typeof(DoneItem))]
    public class ToDoItem : IToDoItem, INotifyPropertyChanged
    {
        private PortableColor _itemColor;

        public ToDoItem()
        {
            CreationDateTime = DateTime.Now;
            _itemColor = PortableColor.FromRgb(255, 255, 255);
        }

        [DataMember]
        public DateTime CreationDateTime { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public PortableColor ItemColor
        {
            get { return _itemColor; }
            set
            {
                if (value.Equals(_itemColor)) return;
                _itemColor = value;
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