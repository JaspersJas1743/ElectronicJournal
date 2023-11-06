using System.IO;

namespace ElectronicJournalAPI.Utilities
{
    internal class TempFile
    {
        private FileInfo _file;
        internal TempFile(string fileName)
        {
            DirectoryInfo directory = new DirectoryInfo(path: Path.Combine(path1: Path.GetTempPath(), path2: "ElectronicJournal"));
            if (!directory.Exists)
                directory.Create();

            _file = new FileInfo(fileName: Path.Combine(path1: directory.FullName, path2: fileName));
        }

        internal bool Exists => _file.Exists;

        internal string FullName => _file.FullName;
    }
}
