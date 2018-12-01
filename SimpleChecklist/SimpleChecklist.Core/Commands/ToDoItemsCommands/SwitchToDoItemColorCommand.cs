using System.Threading.Tasks;
using SimpleChecklist.Common.Entities;

namespace SimpleChecklist.Core.Commands.ToDoItemsCommands
{
    internal class SwitchToDoItemColorCommand : ICommand
    {
        private static readonly string[] Colors = {
            "#ffffff",
            "#ff5a5a",
            "#ffff5a",
            "#5aff5a",
            "#00ffff"
        };

        private readonly ToDoItem _item;

        public SwitchToDoItemColorCommand(ToDoItem item)
        {
            _item = item;
        }

        public void Execute()
        {
            for (int index = 0; index < Colors.Length; index++)
            {
                if (!Colors[index].Equals(_item.Color)) continue;

                var nextColorIndex = index + 1;
                if (nextColorIndex == Colors.Length)
                {
                    nextColorIndex = 0;
                }

                _item.Color = Colors[nextColorIndex];
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