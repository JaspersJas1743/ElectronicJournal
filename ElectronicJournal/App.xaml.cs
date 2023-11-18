using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Logger;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.ViewModels;
using ElectronicJournalAPI.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ElectronicJournal
{
    public partial class App : Application
    {
        private readonly IMessageProvider _message;
        private readonly ILoggerProvider _logger;
        private readonly IConfigProvider _config;

        public App(IMessageProvider message, ILoggerProvider logger, IConfigProvider config)
        {
            InitializeComponent();

            _message = message;
            _logger = logger;
            _config = config;

            if (String.IsNullOrEmpty(value: _config.Get<string>(propertyName: "FolderForDownloads")))
                config.Set(propertyName: "FolderForDownloads", value: Path.Combine(path1: Environment.GetFolderPath(folder: Environment.SpecialFolder.UserProfile), path2: "Downloads"));

            Theme.Init();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            new TempDirectory().CreateIfNotExists();

            MainWindow mainWindow = Program.AppHost.Services.GetService<MainWindow>();
            mainWindow.DataContext = Program.AppHost.Services.GetService<MainWindowVM>();
            mainWindow.Show();
        }

        private async void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Task writeErrorTask = _logger.LogAsync(exception: e.Exception);

            _message.ShowError(text: e.Exception.Message);
            e.Handled = true;

            await writeErrorTask;
        }
    }
}
