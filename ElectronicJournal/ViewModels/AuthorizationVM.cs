using ElectronicJournal.Models;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class AuthorizationVM : VM
    {
        #region Fields
        private readonly IConfigProvider _config;
        private readonly IValidator<AuthorizationModel> _validator;
        private readonly IEventAggregator _eventAggregator;

        private AuthorizationModel _model;

        private readonly Lazy<Command> _authorize;
        private readonly Lazy<Command> _moveToRegistration;
        #endregion Fields

        #region Constructor
        public AuthorizationVM(IValidator<AuthorizationModel> validator, IConfigProvider config, IEventAggregator eventAggregator)
            : base(defaultButtonContent: "Войти")
        {
            _validator = validator;
            _config = config;
            _eventAggregator = eventAggregator;

            _model = new AuthorizationModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _authorize = Command.CreateLazyCommand(action: async _ =>
            {
                await SignIn();
                if (SaveData)
                    _config.SetMany(properties: new Dictionary<string, object> { [nameof(Login)] = Login, [nameof(Password)] = Password });
            },
            canExecute: _ => _validator.Validate(instance: _model).IsValid && CanMoveToAnotherPage);

            _moveToRegistration = Command.CreateLazyCommand(
                action: _ => _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: new ChangeMainWindowContentEventArgs { NewVM = Program.AppHost.Services.GetService<RegistrationVM>() }),
                canExecute: _ => CanMoveToAnotherPage
            );

            Login = _config.Get<string>(propertyName: nameof(Login));
            Password = _config.Get<string>(propertyName: nameof(Password));
            if (new string[] { Login, Password }.Any(x => String.IsNullOrEmpty(value: x)))
                return;

            SaveData = true;
            SignIn();
        }

        private async Task SignIn()
        {
            User user = await ExecuteTask(taskForExecute: _model.SignInAsync);
            MenuVM menuVM = Program.AppHost.Services.GetService<MenuVM>();
            menuVM.User = user;
            _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: new ChangeMainWindowContentEventArgs { NewVM = menuVM });
        }
        #endregion Constructor

        #region Properties
        public string Login
        {
            get => _model.Login;
            set => _model.Login = value;
        }

        public string Password
        {
            get => _model.Password;
            set => _model.Password = value;
        }

        public bool SaveData
        {
            get => _model.SaveData;
            set => _model.SaveData = value;
        }

        public Command Authorize => _authorize.Value;
        public Command MoveToRegistration => _moveToRegistration.Value;
        #endregion Properties
    }
}
