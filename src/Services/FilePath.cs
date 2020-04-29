using System;
using System.Globalization;
using System.IO;

namespace NuKeeper.ProjectReader.Services
{
    public static class FilePath
    {
        public static string RelativeFileName(string fileName, string rootDir)
        {
            if (string.IsNullOrWhiteSpace(rootDir))
            {
                return fileName;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            var separatorChar = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            var rootDirWithSeparator = rootDir.EndsWith(separatorChar, StringComparison.OrdinalIgnoreCase)
                ? rootDir
                : rootDir + Path.DirectorySeparatorChar;

            return fileName.Replace(rootDirWithSeparator, string.Empty, StringComparison.OrdinalIgnoreCase);
        }
    }
}
