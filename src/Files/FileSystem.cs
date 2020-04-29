using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuKeeper.ProjectReader.Logging;

namespace NuKeeper.ProjectReader.Files
{
    public class FileSystem : IFileSystem
    {
        private readonly DirectoryInfo _root;
        private readonly INuKeeperLogger _logger;

        public FileSystem(DirectoryInfo root, INuKeeperLogger logger)
        {
            _root = root;
            _logger = logger;
        }

        public string FullPath => _root.FullName;

        public IReadOnlyCollection<IFile> Find(string pattern)
        {
            try
            {
                return _root
                    .EnumerateFiles(pattern, SearchOption.AllDirectories)
                    .Select(f => new FileOnDisk(f))
                    .ToList();

            }
            catch (IOException ex)
            {
                _logger.Minimal(ex.Message);
                return new List<IFile>();
            }
        }
    }
}
