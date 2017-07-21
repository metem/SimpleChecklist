using System.Collections.Generic;

namespace SimpleChecklist.Common.Interfaces.Utils
{
    public interface IDirectory
    {
        string Path { get; }

        string Name { get; }

        bool Exist { get; }

        IEnumerable<IFile> GetFiles();

        IEnumerable<IDirectory> GetDirectories();

        IDirectory GetChild(string name);

        IDirectory GetParent();
    }
}