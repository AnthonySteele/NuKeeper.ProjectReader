using System.Collections.Generic;

namespace NuKeeper.ProjectReader.Files
{
    public interface IFileSystem
    {
        string FullPath { get; }
        IReadOnlyCollection<IFile> Find(string pattern);
    }
    }
