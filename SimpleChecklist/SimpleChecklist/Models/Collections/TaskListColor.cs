using System.Collections.Generic;
using System.Runtime.Serialization;
using Caliburn.Micro;
using Xamarin.Forms;

namespace SimpleChecklist.Models.Collections
{
    [DataContract]
    public class TaskListColor : PropertyChangedBase
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
                if (_currentColor == value) return;
                _currentColor = value;
                NotifyOfPropertyChange(() => CurrentColor);
            }
        }

        public void MoveToNext()
        {
            CurrentColorIndex++;
        }
    }
}