using System.IO;

namespace NuKeeper.ProjectReader.Files
{
    public interface IFile
    {
        string FullName { get; }

        TextReader ReadContents();
    }
}
