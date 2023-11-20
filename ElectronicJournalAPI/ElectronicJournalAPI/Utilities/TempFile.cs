using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.Utilities
{
    public class TempFile
    {
        #region Fields
        private FileInfo _file;
        #endregion Fields

        #region Constructors
        public TempFile(string file)
            => _file = new FileInfo(fileName: Path.Combine(path1: new TempDirectory().Path, path2: file));

        public TempFile(string folder, string file)
            : this(folder: new TempDirectory(folder: folder), file: file)
        { }

        public TempFile(TempDirectory folder, string file)
            => _file = new FileInfo(fileName: Path.Combine(path1: folder.Path, path2: file));
        #endregion Constructors

        #region Properties
        public bool Exists => File.Exists(path: _file.FullName);

        public long Length => _file.Length;
        public string Name => _file.Name;
        public string FullName => _file.FullName;
        #endregion Properties

        #region Methods
        public void Write(string text, FileMode fileMode = FileMode.Create)
            => Write(buffer: Encoding.Default.GetBytes(s: text), fileMode: fileMode);

        public void Write(byte[] buffer, FileMode fileMode = FileMode.Create)
            => WriteAsync(buffer: buffer, fileMode: fileMode).RunSynchronously();

        public async Task WriteAsync(string text, FileMode fileMode = FileMode.Create, CancellationToken cancellationToken = default)
            => await WriteAsync(buffer: Encoding.Default.GetBytes(s: text), fileMode: fileMode, cancellationToken: cancellationToken);

        public async Task WriteAsync(byte[] buffer, FileMode fileMode = FileMode.Create, CancellationToken cancellationToken = default)
        {
            using (FileStream stream = new FileStream(path: FullName, mode: fileMode))
                await stream.WriteAsync(buffer: buffer, offset: 0, count: buffer.Length, cancellationToken: cancellationToken);
        }

        public void Delete()
            => _file.Delete();
        #endregion Methods
    }
}
