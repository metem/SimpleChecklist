using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;
using SimpleChecklist.Common.Interfaces;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    internal class SwitchToDoItemColorCommand : ICommand
    {
        private static readonly PortableColor[] Colors = {
            PortableColor.FromRgb(255, 255, 255),
            PortableColor.FromRgb(255, 90, 90),
            PortableColor.FromRgb(255, 255, 90),
            PortableColor.FromRgb(90, 255, 90),
            PortableColor.FromRgb(0, 255, 255)
        };

        private readonly IToDoItem _item;

        public SwitchToDoItemColorCommand(IToDoItem item)
        {
            _item = item;
        }

        public void Execute()
        {
            for (int index = 0; index < Colors.Length; index++)
            {
                if (!Colors[index].Equals(_item.ItemColor)) continue;

                var nextColorIndex = index + 1;
                if (nextColorIndex == Colors.Length)
                {
                    nextColorIndex = 0;
                }

                _item.ItemColor = Colors[nextColorIndex];
                break;
            }
        }

        public async Task ExecuteAsync()
        {
            Execute();
            await Task.Yield();
        }
    }
}