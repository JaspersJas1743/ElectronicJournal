using ElectronicJournal.Utilities;
using System;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class MainWindowVM : TrackedObject
	{
		#region Fields
		private readonly Lazy<Command> _exit;
		private readonly Lazy<Command> _toDarkTheme;
		private readonly Lazy<Command> _toLightTheme;
		private readonly Lazy<Command> _minimize;
		private readonly Lazy<Command> _collapse;
		private readonly Lazy<Command> _expand;

		private Visibility _expandVisibility;
		private Visibility _collapseVisibility;
		private Visibility _toLightThemeVisibility;
		private Visibility _toDarkThemeVisibility;

		private TrackedObject _content;

		#endregion Fields

		#region Constructor
		public MainWindowVM()
		{
			_content = new AuthorizationVM();
			_expandVisibility = Visibility.Visible;
			_collapseVisibility = Visibility.Collapsed;
			_toDarkThemeVisibility = GetVisibilityForTheme(theme: Theme.Type.Light);
			_toLightThemeVisibility = GetVisibilityForTheme(theme: Theme.Type.Dark);

			_exit = Command.CreateLazyCommand(action: obj => CloseApp());
			_toLightTheme = Command.CreateLazyCommand(action: obj => ToThemeWindow(newTheme: Theme.Type.Light));
			_toDarkTheme = Command.CreateLazyCommand(action: obj => ToThemeWindow(newTheme: Theme.Type.Dark));
			_minimize = Command.CreateLazyCommand(action: obj => Application.Current.MainWindow.WindowState = WindowState.Minimized);
			_expand = Command.CreateLazyCommand(action: obj => ExpandWindow());
			_collapse = Command.CreateLazyCommand(action: obj => CollapseWindow());
		}
		#endregion Constructor

		#region Properties
		public Command Exit => _exit.Value;

		public Command ToLightTheme => _toLightTheme.Value;

		public Command ToDarkTheme => _toDarkTheme.Value;

		public Command Minimize => _minimize.Value;

		public Command Expand => _expand.Value;

		public Command Collapse => _collapse.Value;

		public Visibility ExpandVisibility
		{
			get => _expandVisibility;
			set
			{
				_expandVisibility = value;
				OnPropertyChanged(nameof(ExpandVisibility));
			}
		}

		public Visibility CollapseVisibility
		{
			get => _collapseVisibility;
			set
			{
				_collapseVisibility = value;
				OnPropertyChanged(nameof(CollapseVisibility));
			}
		}

		public Visibility ToLightThemeVisibility
		{
			get => _toLightThemeVisibility;
			set
			{
				_toLightThemeVisibility = value;
				OnPropertyChanged(nameof(ToLightThemeVisibility));
			}
		}

		public Visibility ToDarkThemeVisibility
		{
			get => _toDarkThemeVisibility;
			set
			{
				_toDarkThemeVisibility = value;
				OnPropertyChanged(nameof(ToDarkThemeVisibility));
			}
		}

		public TrackedObject Content
		{
			get => _content;
			set
			{
				_content = value;
				OnPropertyChanged(nameof(Content));
			}
		}

		public string Name => _content.ToString();

		#endregion Properties

		#region Methods
		private void CloseApp()
		{
			if (Notification.Show(message: "Вы уверены, что хотите закрыть приложение?").Equals(obj: MessageBoxResult.Yes))
				Application.Current.Shutdown();
		}

		private void ExpandWindow()
		{
			Application.Current.MainWindow.WindowState = WindowState.Maximized;
			SwapResizeButtons();
		}

		private void CollapseWindow()
		{
			Application.Current.MainWindow.WindowState = WindowState.Normal;
			SwapResizeButtons();
		}

		private void SwapResizeButtons()
			=> (ExpandVisibility, CollapseVisibility) = (CollapseVisibility, ExpandVisibility);

		private void ToThemeWindow(Theme.Type newTheme)
		{
			Theme.Change(newTheme: newTheme);
			SwapThemeButton();
		}

		private void SwapThemeButton()
			=> (ToLightThemeVisibility, ToDarkThemeVisibility) = (ToDarkThemeVisibility, ToLightThemeVisibility);

		private Visibility GetVisibilityForTheme(Theme.Type theme)
			=> Theme.CurrentTheme == theme ? Visibility.Visible : Visibility.Collapsed;
		#endregion Methods
	}
}
