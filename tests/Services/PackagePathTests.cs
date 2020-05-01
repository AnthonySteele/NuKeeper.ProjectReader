using System.IO;
using NuKeeper.ProjectReader.Data;
using NUnit.Framework;

namespace NuKeeper.ProjectReader.Tests.Services
{
    [TestFixture]
    public class PackagePathTests
    {
        private string _baseDirectory;

        [SetUp]
        public void SetUp()
        {
            _baseDirectory = OsSpecifics.GenerateBaseDirectory();
        }

        [Test]
        public void ConstructorShouldProduceExpectedSimplePropsForProjectFile()
        {
            var sep = Path.DirectorySeparatorChar;

            var path = new PackagePath(
                _baseDirectory,
                $"{sep}checkout1{sep}src{sep}myproj.csproj",
                PackageReferenceType.ProjectFile);

            Assert.That(path.BaseDirectory, Is.EqualTo(_baseDirectory));
            Assert.That(path.RelativePath, Is.EqualTo($"checkout1{sep}src{sep}myproj.csproj"));
            Assert.That(path.PackageReferenceType, Is.EqualTo(PackageReferenceType.ProjectFile));
        }

        [Test]
        public void ConstructorShouldProduceExpectedSimplePropsForPackagesConfigFile()
        {
            var sep = Path.DirectorySeparatorChar;
            var path = new PackagePath(
                _baseDirectory,
                $"{sep}checkout1{sep}src{sep}packages.config",
                PackageReferenceType.PackagesConfig);

            Assert.That(path.BaseDirectory, Is.EqualTo(_baseDirectory));
            Assert.That(path.RelativePath, Is.EqualTo($"checkout1{sep}src{sep}packages.config"));
            Assert.That(path.PackageReferenceType, Is.EqualTo(PackageReferenceType.PackagesConfig));
        }

        [Test]
        public void ConstructorShouldProduceExpectedParsedFullPath()
        {
            var sep = Path.DirectorySeparatorChar;
            var path = new PackagePath(
                _baseDirectory,
                $"checkout1{sep}src{sep}myproj.csproj",
                PackageReferenceType.ProjectFile);


            Assert.That(path.FullName, Is.EqualTo($"{_baseDirectory}{sep}checkout1{sep}src{sep}myproj.csproj"));
        }

        [Test]
        public void ConstructorShouldProduceExpectedParsedPropsWithExtraSlash()
        {
            var sep = Path.DirectorySeparatorChar;
            var path = new PackagePath(
                _baseDirectory,
                $"{sep}checkout1{sep}src{sep}myproj.csproj",
                PackageReferenceType.ProjectFile);


            Assert.That(path.RelativePath, Is.EqualTo($"checkout1{sep}src{sep}myproj.csproj"));
        }

        [Test]
        public void ConstructorShouldProduceExpectedParsedFullPathWithExtraSlash()
        {
            var sep = Path.DirectorySeparatorChar;
            var path = new PackagePath(
                _baseDirectory,
                $"{sep}checkout1{sep}src{sep}myproj.csproj",
                PackageReferenceType.ProjectFile);


            Assert.That(path.BaseDirectory, Is.EqualTo(_baseDirectory));
            Assert.That(path.FullName, Is.EqualTo($"{_baseDirectory}{sep}checkout1{sep}src{sep}myproj.csproj"));
        }
    }
}
