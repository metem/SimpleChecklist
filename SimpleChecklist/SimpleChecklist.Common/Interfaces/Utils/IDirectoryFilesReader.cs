using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleChecklist.Common.Interfaces.Utils
{
    public interface IDirectoryFilesReader : IDirectoryBase
    {
        Task<IEnumerable<IFile>> GetFilesAsync();
    }
}