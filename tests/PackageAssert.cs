using System;
using System.Collections.Generic;
using NuKeeper.ProjectReader.Data;
using NUnit.Framework;

namespace NuKeeper.ProjectReader.Tests
{
    public static class PackageAssert
    {
        public static void AllPopulated(IEnumerable<PackageInProject> packages)
        {
            if (packages == null)
            {
                throw new ArgumentNullException(nameof(packages));
            }

            foreach (var package in packages)
            {
                IsPopulated(package);
            }
        }

        public static void IsPopulated(PackageInProject package)
        {
            Assert.That(package, Is.Not.Null);
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            Assert.That(package.PackageVersionRange, Is.Not.Null);
            Assert.That(package.Version, Is.Not.Null);
            Assert.That(package.Identity, Is.Not.Null);
            Assert.That(package.Path, Is.Not.Null);

            Assert.That(package.Id, Is.Not.Empty);
            Assert.That(package.Version.ToString(), Is.Not.Empty);
            Assert.That(package.ProjectReferences, Is.Not.Null);
        }
    }
}
