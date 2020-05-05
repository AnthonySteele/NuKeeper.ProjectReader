using System.Collections.Generic;
using System.IO;
using NuKeeper.ProjectReader.Data;
using NuKeeper.ProjectReader.Logging;
using NUnit.Framework;

namespace NuKeeper.ProjectReader.Tests
{
    public class ReaderTest
    {
        [Test, TestCaseSource("ListTestCases")]
        public void AllTestCases(string folderName, int expectedCount)
        {
            var testDataFolder = TestDataFolder();

            var path = Path.Join(testDataFolder, folderName);
            var packages = FindAllNuGetPackages(path);

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages.Count, Is.EqualTo(expectedCount));
            PackageAssert.AllPopulated(packages);
        }

        private static IReadOnlyCollection<PackageInProject> FindAllNuGetPackages(string path)
        {
            var reader = new Reader(new NullNuKeeperLogger());
            return reader.FindAllNuGetPackages(path);
        }

        private static string TestDataFolder()
        {
            var current = Directory.GetCurrentDirectory();
            var rootInfo = Directory.GetParent(current).Parent.Parent.Parent;
            var sep = Path.DirectorySeparatorChar;
            return rootInfo.FullName + sep + "TestData";
        }

        // picked up as an "unused field", but it is used via TestCaseSource attribute above
#pragma warning disable CA1823
#pragma warning disable IDE0052
        private static readonly object[] ListTestCases =
        {
            new object[] { "Empty", 0 },
            new object[] { "FullFrameworkCsProj", 1 },
            new object[] { "FullFrameworkVbProj", 1 },
            new object[] { "NetCoreCsProj", 1 },
            new object[] { "NetCoreVbProj", 1 },
            new object[] { "NetCoreFsProj", 1 },
            new object[] { "TwoNetCoreCsProj", 4 },
            new object[] { "DirBuildProps", 1 },
            new object[] { "DirBuildPropsMixed", 2 },
            new object[] { "DirBuildTargets", 1 },
            new object[] { "NuspecFile", 1 },
            new object[] { "ProjectWithExcludedDir", 1 }
        };
    }
}
