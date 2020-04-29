using System.Collections.Generic;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Files;

namespace NuKeeper.ProjectReader.Services
{
    public interface IPackageReferenceFinder
    {
        IReadOnlyCollection<PackageInProject> ReadFile(IFile file, string baseDirectory);

        IReadOnlyCollection<string> GetFilePatterns();
    }
}
