using ElectronicJournalAPI.Utilities;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournal.Utilities.Logger
{
    public class LoggerProvider : ILoggerProvider
    {
        private TempFile _file = new TempFile(file: "logs.txt");

        public void Log(string message)
            => _file.Write(text: message, fileMode: GetFileMode());

        public void Log(Exception exception)
            => Log(message: CreateErrorMessage(exception: exception));

        public async Task LogAsync(string message, CancellationToken cancellationToken = default)
            => await _file.WriteAsync(text: message, fileMode: GetFileMode(), cancellationToken: cancellationToken);

        public async Task LogAsync(Exception exception, CancellationToken cancellationToken = default)
            => await LogAsync(message: CreateErrorMessage(exception: exception), cancellationToken: cancellationToken);

        private FileMode GetFileMode()
            => _file.Exists ? FileMode.Append : FileMode.Create;

        private string CreateErrorMessage(Exception exception)
        {
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.AppendLine($"[Error: {DateTime.Now.ToString("s")}]: ");
            foreach (PropertyInfo property in exception.GetType().GetProperties())
                errorMessage.AppendLine($"\t{property.Name}: {property.GetValue(obj: exception)}");
            errorMessage.AppendLine();
            return errorMessage.ToString();
        }
    }
}
