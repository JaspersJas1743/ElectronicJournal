using System.Windows;
using ElectronicJournal.Utilities;
using ElectronicJournal.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicJournal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Theme.Init();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
			MainWindow mainWindow = Program.AppHost.Services.GetService<MainWindow>();
			mainWindow.DataContext = Program.AppHost.Services.GetService<MainWindowVM>();
			mainWindow.Show();
        }
    }
}
