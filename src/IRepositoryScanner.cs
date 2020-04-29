using System.Collections.Generic;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Files;

namespace NuKeeper.ProjectReader
{
    public interface IRepositoryScanner
    {
        IReadOnlyCollection<PackageInProject> FindAllNuGetPackages(IFileSystem workingFolder);
    }
}
