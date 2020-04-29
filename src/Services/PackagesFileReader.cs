using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Files;
using NuKeeper.ProjectReader.Logging;

namespace NuKeeper.ProjectReader.Services
{
    public class PackagesFileReader : IPackageReferenceFinder
    {
        private readonly PackageInProjectReader _packageInProjectReader;

        public PackagesFileReader(INuKeeperLogger logger)
        {
            _packageInProjectReader = new PackageInProjectReader(logger);
        }

        public IReadOnlyCollection<PackageInProject> ReadFile(IFile file, string baseDirectory)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var relativePath = FilePath.RelativeFileName(file.FullName, baseDirectory);
            var packagePath = new PackagePath(baseDirectory, relativePath, PackageReferenceType.PackagesConfig);
            try
            {
                using var fileContents = file.ReadContents();
                var xml = XDocument.Load(fileContents);
                return Read(xml, packagePath);
            }
            catch (IOException ex)
            {
                throw new ApplicationException($"Unable to parse file {packagePath.FullName}", ex);
            }
        }

        public IReadOnlyCollection<string> GetFilePatterns()
        {
            return new[] { "packages.config" };
        }

        private IReadOnlyCollection<PackageInProject> Read(XDocument xml, PackagePath path)
        {
            var packagesNode = xml.Element("packages");
            if (packagesNode == null)
            {
                return Array.Empty<PackageInProject>();
            }

            var packageNodeList = packagesNode.Elements()
                .Where(x => x.Name == "package");

            var packageList = packageNodeList
                .Select(el => XmlToPackage(el, path))
                .Where(p => p != null)
                .ToList();

            return packageList!;
        }

        private PackageInProject? XmlToPackage(XElement el, PackagePath path)
        {
            var id = el.Attribute("id")?.Value;
            var version = el.Attribute("version")?.Value;

            return _packageInProjectReader.Read(id, version, path, null);
        }
    }
}
