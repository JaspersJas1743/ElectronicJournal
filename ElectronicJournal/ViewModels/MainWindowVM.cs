using ElectronicJournal.Models;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
    public class MainWindowVM : ContentPresenter
    {
        #region Fields
        private readonly IConfigProvider _config;
        private readonly IMessageProvider _message;
        private readonly IEventAggregator _eventAggregator;

        private MainWindowModel _model;

        private readonly Lazy<Command> _exit;
        private readonly Lazy<Command> _changeTheme;
        private readonly Lazy<Command> _minimize;
        private readonly Lazy<Command> _collapse;
        private readonly Lazy<Command> _expand;
        private readonly Lazy<Command> _closing;
        private readonly Lazy<Command> _stateChanged;
        #endregion Fields

        #region Constructors
        public MainWindowVM(IConfigProvider config, IMessageProvider message, IEventAggregator eventAggregator)
        {
            _config = config;
            _message = message;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Subscribe(action: ChangeContent);

            _model = new MainWindowModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            Width = _config.Get<double>(propertyName: nameof(Width));
            Height = _config.Get<double>(propertyName: nameof(Height));
            WindowState = _config.Get<WindowState>(propertyName: nameof(WindowState));
            ExpandVisibility = Visibility.Visible;
            CollapseVisibility = Visibility.Collapsed;
            IsOn = Theme.CurrentTheme == Theme.Type.Dark;
            Content = Program.AppHost.Services.GetService<AuthorizationVM>();

            _exit = Command.CreateLazyCommand(action: _ =>
            {
                MessageWindow.MessageWindowResult exit = _message.Show(
                    text: "Вы уверены, что хотите закрыть приложение?",
                    windowTitle: "Сообщение",
                    image: MessageWindow.MessageWindowImage.Information,
                    buttons: MessageWindow.MessageWindowButton.YesNo
                );

                if (exit.Equals(obj: MessageWindow.MessageWindowResult.Yes))
                    Application.Current.Shutdown();
            });
            
            _changeTheme = Command.CreateLazyCommand(action: obj => Theme.Change(newTheme: Theme.Parse(themeName: obj.ToString())));
            
            _minimize = Command.CreateLazyCommand(action: _ => WindowState = WindowState.Minimized);
            
            _expand = Command.CreateLazyCommand(action: _ =>
            {
                _config.SetMany(properties: new Dictionary<string, object> { [nameof(Width)] = Width, [nameof(Height)] = Height });
                WindowState = WindowState.Maximized;
            });
            
            _collapse = Command.CreateLazyCommand(action: _ => WindowState = WindowState.Normal);
            
            _closing = Command.CreateLazyCommand(action: _ =>
            {
                if (WindowState != WindowState.Maximized)
                    _config.SetMany(properties: new Dictionary<string, object> { [nameof(Width)] = Width, [nameof(Height)] = Height });

                _config.Set(propertyName: nameof(WindowState), value: WindowState);
            });

            _stateChanged = Command.CreateLazyCommand(action: _ => (ExpandVisibility, CollapseVisibility) = (CollapseVisibility, ExpandVisibility));
        }
        #endregion Constructors

        ~MainWindowVM() 
        {
            _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Unsubscribe(subscriber: ChangeContent);
        }

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
            get => _model.ExpandVisibility;
            set => _model.ExpandVisibility = value;
        }

        public Visibility CollapseVisibility
        {
            get => _model.CollapseVisibility;
            set => _model.CollapseVisibility = value;
        }

        public bool IsOn
        {
            get => _model.IsOn;
            set => _model.IsOn = value;
        }

        public double Width
        {
            get => _model.Width;
            set => _model.Width = value;
        }

        public double Height
        {
            get => _model.Height;
            set => _model.Height = value;
        }

        public WindowState WindowState
        {
            get => _model.WindowState;
            set => _model.WindowState = value;
        }
        #endregion Properties

        #region Methods
        private void ChangeContent(ChangeMainWindowContentEventArgs e)
            => Content = e.NewVM;
        #endregion Methods
    }
}
