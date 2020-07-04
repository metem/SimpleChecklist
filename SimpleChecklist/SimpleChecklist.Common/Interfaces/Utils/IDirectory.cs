using System.Collections.Generic;

namespace SimpleChecklist.Common.Interfaces.Utils
{
    public interface IDirectory : IDirectoryFilesReader, IDirectoryBase
    {
        IDirectory GetChild(string name);

        IEnumerable<IDirectory> GetDirectories();

        IDirectory GetParent();
    }
}