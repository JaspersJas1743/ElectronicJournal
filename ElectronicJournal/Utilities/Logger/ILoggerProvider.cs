using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournal.Utilities.Logger
{
    public interface ILoggerProvider
    {
        void Log(string message);

        void Log(Exception exception);

        Task LogAsync(string message, CancellationToken cancellationToken = default);

        Task LogAsync(Exception exception, CancellationToken cancellationToken = default);

    }
}
