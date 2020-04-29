using System;
using System.IO;
using NuGet.Common;
using NuKeeper.ProjectReader.Logging;
using NUnit.Framework;

namespace NuKeeper.ProjectReader.Tests
{
    public class ReaderTest
    {
        [Test]
        public void ReadTempFolder()
        {
            var path = "c:\\temp";
            var packages = Reader.FindAllNuGetPackages(path, new NullNuKeeperLogger());

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages, Is.Empty);
        }

        [Test]
        public void ReadNetCoreProject()
        {
           var testDataFolder = TestDataFolder();

            var path = Path.Join(testDataFolder, "NetCoreCsProj");
            var packages = Reader.FindAllNuGetPackages(path, new NullNuKeeperLogger());

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages, Is.Not.Empty);
        }

        [Test]
        public void ReadDirBuildPropsProject()
        {
            var testDataFolder = TestDataFolder();

            var path = Path.Join(testDataFolder, "DirBuildProps");
            var packages = Reader.FindAllNuGetPackages(path, new NullNuKeeperLogger());

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages, Is.Not.Empty);
        }


        private static string TestDataFolder()
        {
            var current = Directory.GetCurrentDirectory();
            var rootInfo = Directory.GetParent(current).Parent.Parent.Parent;
            var sep = Path.DirectorySeparatorChar;
            return rootInfo.FullName + sep + "TestData";
        }
    }
}
