using System.IO;
using System.Text;
using NuKeeper.ProjectReader.Files;

namespace NuKeeper.ProjectReader.Tests
{
    public class TestFile : IFile
    {
        private readonly string _contents;

        public TestFile(string fullName, string contents)
        {
            FullName = fullName;
            _contents = contents;
        }

        public string FullName { get; }

        public TextReader ReadContents()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(_contents));
            return new StreamReader(stream);
        }
    }
}
