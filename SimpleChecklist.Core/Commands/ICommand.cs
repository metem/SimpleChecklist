using System.Threading.Tasks;

namespace SimpleChecklist.Core.Commands
{
    interface ICommand
    {
        Task ExecuteAsync();
    }
}
