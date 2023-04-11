using ElectronicJournal.Utilities;
using System;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class MainWindowVM : BaseVM
	{
		private readonly Lazy<Command> _exit;
		private readonly Lazy<Command> _changeTheme;
		private readonly Lazy<Command> _deactivate;
		private readonly Lazy<Command> _expand;
		private BaseVM _content;

		public MainWindowVM()
		{
			_exit = Command.CreateLazyCommand(action: obj => CloseApp());
			_changeTheme = Command.CreateLazyCommand(action: obj => Theme.Change());
			_deactivate = Command.CreateLazyCommand(action: obj => Application.Current.MainWindow.WindowState = WindowState.Minimized);
			_expand = Command.CreateLazyCommand(action: obj => ExpandWindow());
			_content = new AuthorizationVM();
		}

		public Command Exit => _exit.Value;

		public Command ChangeTheme => _changeTheme.Value;

		public Command Deactivate => _deactivate.Value;

		public Command Expand => _expand.Value;

		public BaseVM Content
		{
			get => _content;
			set
			{
				_content = value;
				OnPropertyChanged("Content");
			}
		}

		public string Name => _content.ToString();

		private void CloseApp()
		{
			if (MessageBox.Show("Закрыть приложение?", "", MessageBoxButton.YesNo, MessageBoxImage.Question).Equals(MessageBoxResult.Yes))
				Application.Current.Shutdown();
		}

		private void ExpandWindow()
			=> Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
	}
}
