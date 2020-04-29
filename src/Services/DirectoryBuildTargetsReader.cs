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
    public class DirectoryBuildTargetsReader : IPackageReferenceFinder
    {
        private readonly PackageInProjectReader _packageInProjectReader;

        public DirectoryBuildTargetsReader(INuKeeperLogger logger)
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

            var packagePath = new PackagePath(baseDirectory, relativePath, PackageReferenceType.DirectoryBuildTargets);
            try
            {
                using var fileContents = file.ReadContents();
                var xml = XDocument.Load(fileContents);
                return Read(xml, packagePath);
            }
            catch (IOException ex)
            {
                throw new NuKeeperException($"Unable to parse file {packagePath.FullName}", ex);
            }
        }

        public IReadOnlyCollection<string> GetFilePatterns()
        {
            return new[] { "Directory.Build.props", "Directory.Build.targets", "Packages.props" };
        }

        private IReadOnlyCollection<PackageInProject> Read(XDocument xml, PackagePath path)
        {
            var packagesNode = xml.Element("Project")?.Elements("ItemGroup");
            if (packagesNode == null)
            {
                return Array.Empty<PackageInProject>();
            }

            var packageNodeList = packagesNode.Elements("PackageReference");

            var packageList = packageNodeList
                .Select(el => XmlToPackage(el, path))
                .Where(p => p != null)
                .ToList();

            return packageList!;
        }

        private PackageInProject? XmlToPackage(XElement el, PackagePath path)
        {
            var id = el.Attribute("Include")?.Value;
            if (id == null)
            {
                id = el.Attribute("Update")?.Value;
            }
            var version = el.Attribute("Version")?.Value;

            return _packageInProjectReader.Read(id, version, path, null);
        }
    }
}
