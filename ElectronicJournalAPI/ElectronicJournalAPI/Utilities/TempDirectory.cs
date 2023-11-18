using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ElectronicJournalAPI.Utilities
{
    public class TempDirectory
    {
        private DirectoryInfo _directoryInfo;

        public TempDirectory(string folder)
            => _directoryInfo = new DirectoryInfo(path: System.IO.Path.Combine(path1: System.IO.Path.GetTempPath(), path2: "ElectronicJournal", path3: folder));

        public TempDirectory()
            => _directoryInfo = new DirectoryInfo(path: System.IO.Path.Combine(path1: System.IO.Path.GetTempPath(), path2: "ElectronicJournal"));

        public bool Exists => Directory.Exists(path: _directoryInfo.FullName);
        public string Path => _directoryInfo.FullName;

        public void CreateIfNotExists()
        {
            if (!Exists)
                _directoryInfo.Create();
        }

        public bool TryFindFile(string fileName, out TempFile file, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            file = null;
            string fileOrDefault = _directoryInfo.GetFiles().Where(f => Regex.IsMatch(input: f.Name, pattern: $"^{fileName}")).SingleOrDefault()?.FullName;

            if (fileOrDefault == null)
                return false;

            file = new TempFile(folder: this, file: fileOrDefault);
            return true;
        }

        public TempFile FindFile(string fileName, SearchOption searchOption = SearchOption.TopDirectoryOnly)
            => new TempFile(folder: this, file: _directoryInfo.GetFiles().Where(f => Regex.IsMatch(input: f.Name, pattern: $"^{fileName}")).Single().FullName);
    }
}
