using System;
using System.IO;
using NuKeeper.ProjectReader.Services;
using NUnit.Framework;

namespace NuKeeper.ProjectReader.Tests.Services
{
    [TestFixture]
    public class DirectoryExclusionsTests
    {
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}NuKeeper.csproj")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}GitHub{sep}IGithub.cs")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}dustbin.cs")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}objections.cs")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}Packages.cs")]
        public void ShouldBeIncluded(string pathTemplate)
        {
            if (string.IsNullOrWhiteSpace(pathTemplate))
            {
                throw new ArgumentNullException(nameof(pathTemplate));
            }

            string path = ExpandTemplate(pathTemplate);

            var exclusions = new DirectoryExclusions();
            var actual = exclusions.PathIsExcluded(path);

            Assert.That(actual, Is.False);
        }

        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}bin{sep}debug{sep}net461{sep}config.json")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}obj{sep}debug{sep}net461{sep}config.json")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}NuKeeper{sep}packages{sep}foo.text")]
        [TestCase("C:{sep}Code{sep}NuKeeper{sep}.git{sep}config")]
        public void ShouldBeExcluded(string pathTemplate)
        {
            if (string.IsNullOrWhiteSpace(pathTemplate))
            {
                throw new ArgumentNullException(nameof(pathTemplate));
            }

            string path = ExpandTemplate(pathTemplate);

            var exclusions = new DirectoryExclusions();
            var actual = exclusions.PathIsExcluded(path);

            Assert.That(actual, Is.True);
        }

        private static string ExpandTemplate(string pathTemplate)
        {
            return pathTemplate.Replace("{sep}", $"{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase);
        }
    }
}
