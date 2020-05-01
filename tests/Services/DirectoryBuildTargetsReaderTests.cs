using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Logging;
using NuKeeper.ProjectReader.Services;
using NUnit.Framework;

namespace NuKeeper.ProjectReader.Tests.Services
{
    [TestFixture]
    public class DirectoryBuildTargetsReaderTests
    {
        const string PackagesFileWithSinglePackage =
            @"<Project><ItemGroup><PackageReference Include=""foo"" Version=""1.2.3.4"" /></ItemGroup></Project>";

        private const string PackagesFileWithTwoPackages = @"<Project><ItemGroup>
<PackageReference Include=""foo"" Version=""1.2.3.4"" />
<PackageReference Update=""bar"" Version=""2.3.4.5"" /></ItemGroup></Project>";

        [Test]
        public void EmptyPackagesListShouldBeParsed()
        {
            const string emptyContents =
                @"<Project/>";

            var packages = ParseContent(emptyContents);

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages, Is.Empty);
        }


        [Test]
        public void SinglePackageShouldBeRead()
        {
            var packages = ParseContent(PackagesFileWithSinglePackage);

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages, Is.Not.Empty);
        }

        [Test]
        public void SinglePackageShouldBePopulated()
        {
            var packages = ParseContent(PackagesFileWithSinglePackage);

            var package = packages.FirstOrDefault();
            PackageAssert.IsPopulated(package);
        }

        [Test]
        public void SinglePackageShouldBeCorrect()
        {
            var packages = ParseContent(PackagesFileWithSinglePackage);

            var package = packages.FirstOrDefault();

            Assert.That(package, Is.Not.Null);
            Assert.That(package.Id, Is.EqualTo("foo"));
            Assert.That(package.Version, Is.EqualTo(new NuGetVersion("1.2.3.4")));
            Assert.That(package.Path.PackageReferenceType, Is.EqualTo(PackageReferenceType.DirectoryBuildTargets));
        }

        [Test]
        public void TwoPackagesShouldBePopulated()
        {
            var packages = ParseContent(PackagesFileWithTwoPackages).ToList();

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages.Count, Is.EqualTo(2));

            PackageAssert.IsPopulated(packages[0]);
            PackageAssert.IsPopulated(packages[1]);
        }

        [Test]
        public void TwoPackagesShouldBeRead()
        {
            var packages = ParseContent(PackagesFileWithTwoPackages).ToList();

            Assert.That(packages.Count, Is.EqualTo(2));

            Assert.That(packages[0].Id, Is.EqualTo("foo"));
            Assert.That(packages[0].Version, Is.EqualTo(new NuGetVersion("1.2.3.4")));

            Assert.That(packages[1].Id, Is.EqualTo("bar"));
            Assert.That(packages[1].Version, Is.EqualTo(new NuGetVersion("2.3.4.5")));
        }

        [Test]
        public void ResultIsReiterable()
        {
            var packages = ParseContent(PackagesFileWithTwoPackages);

            PackageAssert.AllPopulated(packages);
            Assert.That(packages.Select(p => p.Path), Is.All.Not.Null);
        }

        [Test]
        public void WhenOnePackageCannotBeRead_TheOthersAreStillRead()
        {
            var badVersion = PackagesFileWithTwoPackages.Replace("1.2.3.4", "notaversion", StringComparison.OrdinalIgnoreCase);

            var packages = ParseContent(badVersion)
                .ToList();

            Assert.That(packages.Count, Is.EqualTo(1));
            PackageAssert.IsPopulated(packages[0]);
        }

        private static IReadOnlyCollection<PackageInProject> ParseContent(string fileContents)
        {
            var file = new TestFile("testfile", fileContents);
            IPackageReferenceFinder reader = new DirectoryBuildTargetsReader(new NullNuKeeperLogger());
            return reader.ReadFile(file, "c://code//test");
        }
    }
}
