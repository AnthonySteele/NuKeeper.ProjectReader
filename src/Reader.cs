using System.Collections.Generic;
using System.IO;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Files;
using NuKeeper.ProjectReader.Logging;
using NuKeeper.ProjectReader.Services;

namespace NuKeeper.ProjectReader
{
    public class Reader : IReader
    {
        private readonly INuKeeperLogger _logger;

        public Reader(INuKeeperLogger logger)
        {
            _logger = logger;
        }

        public IReadOnlyCollection<PackageInProject> FindAllNuGetPackages(string path)
        {
            var folder = new FileSystem(new DirectoryInfo(path), _logger);
            var scanner = BuildScanner(_logger);
            return scanner.FindAllNuGetPackages(folder);
        }

        private static RepositoryScanner BuildScanner(INuKeeperLogger logger)
        {
            return new RepositoryScanner(new DirectoryExclusions(), logger);
        }
    }
}
