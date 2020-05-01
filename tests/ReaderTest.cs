using System.IO;
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
            var packages = Reader.FindAllNuGetPackages(path, new NullNuKeeperLogger());

            Assert.That(packages, Is.Not.Null);
            Assert.That(packages.Count, Is.EqualTo(expectedCount));
            PackageAssert.AllPopulated(packages);
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
