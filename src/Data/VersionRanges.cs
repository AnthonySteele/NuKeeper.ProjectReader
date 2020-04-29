using NuGet.Versioning;

namespace NuKeeper.ProjectReader.Data
{
    public static class VersionRanges
    {
        public static NuGetVersion? SingleVersion(VersionRange range)
        {
            if (range == null)
            {
                return null;
            }

            if (range.IsFloating || range.HasLowerAndUpperBounds)
            {
                return null;
            }

            return range.MinVersion;
        }
    }
}
