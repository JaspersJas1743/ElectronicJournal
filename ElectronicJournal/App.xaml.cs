using System.Windows;
using ElectronicJournal.Utilities;

namespace ElectronicJournal
{
    public partial class App : Application
    {
        private readonly MainWindow _mainWindow;

        public App()
        {
            InitializeComponent();
            Theme.Init();
            _mainWindow = new MainWindow();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
            => _mainWindow.Show();
    }
}
