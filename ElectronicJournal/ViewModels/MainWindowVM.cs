using ElectronicJournal.Utilities;
using ElectronicJournal.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private readonly Lazy<Command> _exit;
        private readonly Lazy<Command> _changeTheme;
        private readonly Lazy<Command> _deactivate;
        private readonly Lazy<Command> _expand;
        private readonly Page _content;

        public MainWindowVM()
        {
            _exit = Command.CreateCommand(action: obj => Application.Current.Shutdown());
            _changeTheme = Command.CreateCommand(action: obj => Theme.Change());
            _deactivate = Command.CreateCommand(action: obj => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            _expand = Command.CreateCommand(action: obj => ExpandWindow());
            _content = new Authorization();
        }

        public Command Exit => _exit.Value;

        public Command ChangeTheme => _changeTheme.Value;

        public Command Deactivate => _deactivate.Value;

        public Command Expand => _expand.Value;

        public Page Content => _content;

        private void ExpandWindow()
            => Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName));
    }
}
