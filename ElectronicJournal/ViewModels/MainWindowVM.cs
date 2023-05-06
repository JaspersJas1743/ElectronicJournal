using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities;
using System;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class MainWindowVM : TrackedObject
	{
		#region Fields
		private readonly Lazy<Command> _exit;
		private readonly Lazy<Command> _changeTheme;
		private readonly Lazy<Command> _minimize;
		private readonly Lazy<Command> _collapse;
		private readonly Lazy<Command> _expand;

		private Visibility _collapseVisibility;
		private Visibility _expandVisibility;
		private TrackedObject _content;
		private bool _isOn;

		#endregion Fields

		#region Constructors
		public MainWindowVM()
		{
			_content = new AuthorizationVM();
			_expandVisibility = Visibility.Visible;
			_collapseVisibility = Visibility.Collapsed;
			_isOn = Theme.CurrentTheme == Theme.Type.Dark;
			Application.Current.MainWindow.StateChanged += OnMainWindowStateChanged;

			_exit = Command.CreateLazyCommand(action: obj => CloseApp());
			_changeTheme = Command.CreateLazyCommand(action: obj => Theme.Change(newTheme: Theme.Parse(themeName: obj.ToString())));
			_minimize = Command.CreateLazyCommand(action: obj => Application.Current.MainWindow.WindowState = WindowState.Minimized);
			_expand = Command.CreateLazyCommand(action: obj => ExpandWindow());
			_collapse = Command.CreateLazyCommand(action: obj => CollapseWindow());
		}
		#endregion Constructors

		#region Properties
		public Command Exit => _exit.Value;

		public Command ChangeTheme => _changeTheme.Value;

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

		public TrackedObject Content
		{
			get => _content;
			set
			{
				_content = value;
				OnPropertyChanged(nameof(Content));
			}
		}

		public bool IsOn
		{
			get => _isOn;
			set
			{
				_isOn = value;
				OnPropertyChanged(nameof(IsOn));
			}
		}
		#endregion Properties

		#region Methods
		private void CloseApp()
		{

			if (MessageWindow.Show(text: "Вы уверены, что хотите закрыть приложение?",
				image: MessageWindow.MessageWindowImage.Information,
				buttons: MessageWindow.MessageWindowButton.YesNo).Equals(obj: MessageWindow.MessageWindowResult.Yes))
				Application.Current.Shutdown();
		}

		private void ExpandWindow()
			=> Application.Current.MainWindow.WindowState = WindowState.Maximized;

		private void CollapseWindow()
			=> Application.Current.MainWindow.WindowState = WindowState.Normal;

		private void OnMainWindowStateChanged(object sender, EventArgs e)
			=> SwapResizeButtons();

		private void SwapResizeButtons()
			=> (ExpandVisibility, CollapseVisibility) = (CollapseVisibility, ExpandVisibility);
		#endregion Methods
	}
}
