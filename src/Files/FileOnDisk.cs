using System;
using System.IO;

namespace NuKeeper.ProjectReader.Files
{
    public class FileOnDisk: IFile
    {
        private readonly FileInfo _fileInfo;

        public FileOnDisk(FileInfo fileInfo)
        {
            _fileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }

        public string FullName => _fileInfo.FullName;

        public TextReader ReadContents()
        {
            return  new StreamReader(File.OpenRead(FullName));
        }
    }
}
