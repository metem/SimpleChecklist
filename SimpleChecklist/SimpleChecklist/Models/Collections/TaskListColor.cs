using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SimpleChecklist.Properties;
using Xamarin.Forms;

namespace SimpleChecklist.Models.Collections
{
    [DataContract]
    public class TaskListColor : INotifyPropertyChanged
    {
        private static readonly List<Color> Colors = new List<Color>
        {
            Color.White,
            Color.FromRgb(255, 90, 90),
            Color.FromRgb(255, 255, 90),
            Color.FromRgb(90, 255, 90),
            Color.FromRgb(0, 255, 255)
        };

        private Color _currentColor;
        private int _currentColorIndex;

        public TaskListColor()
        {
            CurrentColor = Colors[CurrentColorIndex];
        }

        [DataMember]
        private int CurrentColorIndex
        {
            get { return _currentColorIndex; }
            set
            {
                _currentColorIndex = value < Colors.Count ? value : 0;
                CurrentColor = Colors[_currentColorIndex];
            }
        }

        public Color CurrentColor
        {
            get { return _currentColor; }
            set
            {
                _currentColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void MoveToNext()
        {
            CurrentColorIndex++;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}