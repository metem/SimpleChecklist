using System.Threading.Tasks;

namespace SimpleChecklist.Core.Commands
{
    internal interface ICommand
    {
        Task ExecuteAsync();
    }
}