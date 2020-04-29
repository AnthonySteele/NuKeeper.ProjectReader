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
    public class ProjectFileReader : IPackageReferenceFinder
    {
        private const string VisualStudioLegacyProjectNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private readonly PackageInProjectReader _packageInProjectReader;

        public ProjectFileReader(INuKeeperLogger logger)
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

            try
            { 
                using var fileContents = file.ReadContents();
                var xml = XDocument.Load(fileContents);
                var xmlNamespace = xml.Root.GetDefaultNamespace();
                var packagePath = CreatePackagePath(xmlNamespace, baseDirectory, relativePath);

                return Read(xml, xmlNamespace, packagePath);
            }
            catch (IOException ex)
            {
                throw new ApplicationException($"Unable to parse file {file.FullName}", ex);
            }
        }

        public IReadOnlyCollection<string> GetFilePatterns()
        {
            return new[] { "*.csproj", "*.vbproj", "*.fsproj" };
        }

        private IReadOnlyCollection<PackageInProject> Read(XDocument xml, XNamespace xmlNamespace, PackagePath packagePath)
        {
            var project = xml.Element(xmlNamespace + "Project");

            if (project == null)
            {
                return Array.Empty<PackageInProject>();
            }

            var itemGroups = project
                .Elements(xmlNamespace + "ItemGroup")
                .ToList();

            var projectRefs = itemGroups
                .SelectMany(ig => ig.Elements(xmlNamespace + "ProjectReference"))
                .Select(el => MakeProjectPath(el, packagePath.FullName))
                .ToList();

            var packageRefElements = itemGroups.SelectMany(ig => ig.Elements(xmlNamespace + "PackageReference"));

            var packageRefs = packageRefElements
                .Select(el => XmlToPackage(xmlNamespace, el, packagePath, projectRefs))
                .Where(p => p != null)
                .ToList();

            return packageRefs!;
        }

        private static string MakeProjectPath(XElement el, string currentPath)
        {
            var relativePath = el.Attribute("Include")?.Value;

            var currentDir = Path.GetDirectoryName(currentPath);
            var combinedPath = Path.Combine(currentDir, relativePath);

            // combined path can still have "\..\" parts to it, need to canonicalise
            return Path.GetFullPath(combinedPath);
        }

        private static PackagePath CreatePackagePath(XNamespace xmlNamespace, string baseDirectory, string relativePath)
        {
            return xmlNamespace.NamespaceName == VisualStudioLegacyProjectNamespace
                ? new PackagePath(baseDirectory, relativePath, PackageReferenceType.ProjectFileOldStyle)
                : new PackagePath(baseDirectory, relativePath, PackageReferenceType.ProjectFile);
        }

        private PackageInProject? XmlToPackage(XNamespace ns, XElement el,
            PackagePath path, IEnumerable<string> projectReferences)
        {
            var id = el.Attribute("Include")?.Value;
            var version = el.Attribute("Version")?.Value ?? el.Element(ns + "Version")?.Value;

            return _packageInProjectReader.Read(id, version, path, projectReferences);
        }
    }
}
