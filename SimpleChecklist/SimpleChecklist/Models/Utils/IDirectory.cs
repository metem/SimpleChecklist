using System.Collections.Generic;

namespace SimpleChecklist.Models.Utils
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