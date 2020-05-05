using System.Collections.Generic;
using NuKeeper.ProjectReader.Data;

namespace NuKeeper.ProjectReader
{
    public interface IReader
    {
        IReadOnlyCollection<PackageInProject> FindAllNuGetPackages(string path);
    }
}
