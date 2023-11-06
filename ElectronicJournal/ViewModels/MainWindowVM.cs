using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.ViewModels.Tools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
    public class MainWindowVM : ContentPresenter
    {
        #region Fields
        private readonly IConfigProvider _configProvider;

        private readonly Lazy<Command> _exit;
        private readonly Lazy<Command> _changeTheme;
        private readonly Lazy<Command> _minimize;
        private readonly Lazy<Command> _collapse;
        private readonly Lazy<Command> _expand;
        private readonly Lazy<Command> _closing;
        private readonly Lazy<Command> _stateChanged;

        private Visibility _collapseVisibility;
        private Visibility _expandVisibility;
        private double _width;
        private double _height;
        private bool _isOn;
        #endregion Fields

        #region Constructors
        public MainWindowVM(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            Width = _configProvider.Get<Double>(propertyName: nameof(Width));
            Height = _configProvider.Get<Double>(propertyName: nameof(Height));
            Content = Program.AppHost.Services.GetService<AuthorizationVM>();
            ExpandVisibility = Visibility.Visible;
            CollapseVisibility = Visibility.Collapsed;
            IsOn = Theme.CurrentTheme == Theme.Type.Dark;

            _exit = Command.CreateLazyCommand(action: _ => CloseApp());
            _changeTheme = Command.CreateLazyCommand(action: obj => Theme.Change(newTheme: Theme.Parse(themeName: obj.ToString())));
            _minimize = Command.CreateLazyCommand(action: _ => Application.Current.MainWindow.WindowState = WindowState.Minimized);
            _expand = Command.CreateLazyCommand(action: _ => ExpandWindow());
            _collapse = Command.CreateLazyCommand(action: _ => CollapseWindow());
            _closing = Command.CreateLazyCommand(action: _ =>
            {
                _configProvider.Set(propertyName: nameof(Width), value: Width);
                _configProvider.Set(propertyName: nameof(Height), value: Height);
            });
            _stateChanged = Command.CreateLazyCommand(action: _ => SwapResizeButtons());
        }
        #endregion Constructors

        #region Properties
        public Command Exit => _exit.Value;

        public Command ChangeTheme => _changeTheme.Value;

        public Command Minimize => _minimize.Value;

        public Command Expand => _expand.Value;

        public Command Collapse => _collapse.Value;
        public Command Closing => _closing.Value;
        public Command StateChanged => _stateChanged.Value;

        public Visibility ExpandVisibility
        {
            get => _expandVisibility;
            set
            {
                _expandVisibility = value;
                OnPropertyChanged(propertyName: nameof(ExpandVisibility));
            }
        }

        public Visibility CollapseVisibility
        {
            get => _collapseVisibility;
            set
            {
                _collapseVisibility = value;
                OnPropertyChanged(propertyName: nameof(CollapseVisibility));
            }
        }
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                OnPropertyChanged(propertyName: nameof(IsOn));
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(propertyName: nameof(Width));
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(propertyName: nameof(Height));
            }
        }
        #endregion Properties

        #region Methods
        private void CloseApp()
        {
            if (MessageWindow.Show(text: "Вы уверены, что хотите закрыть приложение?",
                windowTitle: "Сообщение",
                image: MessageWindow.MessageWindowImage.Information,
                buttons: MessageWindow.MessageWindowButton.YesNo).Equals(obj: MessageWindow.MessageWindowResult.Yes))
                Application.Current.Shutdown();
        }

        private void ExpandWindow()
            => Application.Current.MainWindow.WindowState = WindowState.Maximized;

        private void CollapseWindow()
            => Application.Current.MainWindow.WindowState = WindowState.Normal;

        private void SwapResizeButtons()
            => (ExpandVisibility, CollapseVisibility) = (CollapseVisibility, ExpandVisibility);
        #endregion Methods
    }
}
