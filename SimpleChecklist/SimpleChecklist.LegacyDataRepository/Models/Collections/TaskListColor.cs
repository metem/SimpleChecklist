using System.Collections.Generic;
using System.Runtime.Serialization;
using Xamarin.Forms;

namespace SimpleChecklist.Models.Collections
{
    [DataContract]
    public class TaskListColor
    {
        private static readonly List<Color> Colors = new List<Color>
        {
            Color.White,
            Color.FromRgb(255, 90, 90),
            Color.FromRgb(255, 255, 90),
            Color.FromRgb(90, 255, 90),
            Color.FromRgb(0, 255, 255)
        };

        private int _currentColorIndex;

        public TaskListColor()
        {
            CurrentColor = Colors[CurrentColorIndex];
        }

        [DataMember]
        private int CurrentColorIndex
        {
            get => _currentColorIndex;
            set
            {
                _currentColorIndex = value < Colors.Count ? value : 0;
                CurrentColor = Colors[_currentColorIndex];
            }
        }

        public Color CurrentColor { get; set; }

        public void MoveToNext()
        {
            CurrentColorIndex++;
        }
    }
}