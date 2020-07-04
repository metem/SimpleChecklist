namespace SimpleChecklist.Common.Interfaces.Utils
{
    public interface IDirectoryBase
    {
        bool Exist { get; }
        string Name { get; }
        string Path { get; }
    }
}