namespace NuKeeper.ProjectReader.Services
{
    public interface IDirectoryExclusions
    {
        bool PathIsExcluded(string path);
    }
}
