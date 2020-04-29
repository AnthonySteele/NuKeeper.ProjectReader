using System;
using System.Collections.Generic;
using System.Linq;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Files;
using NuKeeper.ProjectReader.Logging;

namespace NuKeeper.ProjectReader.Services
{
    public class RepositoryScanner : IRepositoryScanner
    {
        private readonly IReadOnlyCollection<IPackageReferenceFinder> _finders;
        private readonly IDirectoryExclusions _directoryExclusions;

        public RepositoryScanner(IDirectoryExclusions directoryExclusions, INuKeeperLogger logger)
        {
            _directoryExclusions = directoryExclusions ?? throw new ArgumentNullException(nameof(directoryExclusions));

            if (logger == null)
            {
               throw new ArgumentNullException(nameof(logger));
            }

            _finders = BuildFinders(logger);
        }

        private static IReadOnlyCollection<IPackageReferenceFinder> BuildFinders(INuKeeperLogger logger)
        {
            return new List<IPackageReferenceFinder>
            {
                new ProjectFileReader(logger),
                new PackagesFileReader(logger),
                new NuspecFileReader(logger),
                new DirectoryBuildTargetsReader(logger)
            };
        }

        public IReadOnlyCollection<PackageInProject> FindAllNuGetPackages(IFileSystem workingFolder)
        {
            return _finders
                .SelectMany(f => FindPackages(workingFolder, f))
                .ToList();
        }

        private IEnumerable<PackageInProject> FindPackages(IFileSystem workingFolder,
            IPackageReferenceFinder packageReferenceFinder)
        {
            var files = packageReferenceFinder
                .GetFilePatterns()
                .SelectMany(workingFolder.Find);

            var filesInUsableDirectories =
                files.Where(f => !_directoryExclusions.PathIsExcluded(f.FullName));

            return filesInUsableDirectories
                .SelectMany(f => ReadFilePackages(workingFolder, packageReferenceFinder, f));
        }

        private static IReadOnlyCollection<PackageInProject> ReadFilePackages(
            IFileSystem workingFolder,
            IPackageReferenceFinder packageReferenceFinder,
            IFile  file)
        {
            return packageReferenceFinder.ReadFile(file, workingFolder.FullPath);
        }
    }
}
