using System.Collections.Generic;

namespace SimpleChecklist.Models.Utils
{
    public interface IDirectory
    {
        IEnumerable<IFile> GetFiles();

        IEnumerable<IDirectory> GetDirectories();

        string Path { get; }

        string Name { get; }

        bool Exist { get; }

        IDirectory GetChild(string name);

        IDirectory GetParent();
    }
}