using ElectronicJournal.Utilities.Config;
using System;
using System.ComponentModel;
using System.Windows;

namespace ElectronicJournal.Resources.Windows
{
    public partial class MainWindow : Window
    {
        private readonly IConfigProvider _configProvider;

        public MainWindow(IConfigProvider configProvider)
        {
            InitializeComponent();
            _configProvider = configProvider;
            this.Width = _configProvider.Get<Double>(propertyName: nameof(this.Width));
            this.Height = _configProvider.Get<Double>(propertyName: nameof(this.Height));
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                (this.Width, this.Height) = (this.MinWidth, this.MinHeight);
            _configProvider.Set(propertyName: nameof(this.Width), value: this.Width);
            _configProvider.Set(propertyName: nameof(this.Height), value: this.Height);
        }
    }
}
