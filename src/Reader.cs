using System.Collections.Generic;
using System.IO;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Files;
using NuKeeper.ProjectReader.Logging;
using NuKeeper.ProjectReader.Services;

namespace NuKeeper.ProjectReader
{
    public static class Reader
    {
        public static IReadOnlyCollection<PackageInProject> FindAllNuGetPackages(string path, INuKeeperLogger logger)
        {
            var folder = new FileSystem(new DirectoryInfo(path), logger);
            var scanner = BuildScanner(logger);

            return scanner.FindAllNuGetPackages(folder);
        }

        private static RepositoryScanner BuildScanner(INuKeeperLogger logger)
        {
            return new RepositoryScanner(new DirectoryExclusions(), logger);
        }
    }
}
