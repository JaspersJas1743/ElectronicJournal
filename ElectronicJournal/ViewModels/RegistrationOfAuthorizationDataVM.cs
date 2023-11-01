using ElectronicJournal.Models;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class RegistrationOfAuthorizationDataVM : VM
    {
        #region Fields
        private readonly INavigationProvider _navigationProvider;

        private readonly Lazy<Command> _registrationCommand;
        private readonly Lazy<Command> _backCommand;
        private readonly IValidator<RegistrationOfAuthorizationDataModel> _registrationOfAuthorizationDataModelValidator;

        private RegistrationOfAuthorizationDataModel _model;
        #endregion Fields

        #region Constructors
        public RegistrationOfAuthorizationDataVM(INavigationProvider navigationProvider, IValidator<RegistrationOfAuthorizationDataModel> validator)
            : base(defaultButtonContent: "Зарегистрироваться")
        {
            _navigationProvider = navigationProvider;
            _model = new RegistrationOfAuthorizationDataModel();
            _registrationOfAuthorizationDataModelValidator = validator;

            _registrationCommand = Command.CreateLazyCommand(action: async _ =>
            {
                try
                {
                    await ExecuteTask(taskForExecute: SignUpAsync);
                    _navigationProvider.MoveTo<AuthorizationVM>();
                }
                catch (Exception ex)
                {
                    MessageWindow.ShowError(text: ex.Message);
                }
            }, canExecute: _ => validator.Validate(instance: _model).IsValid && CanMoveToAnotherPage);

            _backCommand = Command.CreateLazyCommand(
                action: _ => _navigationProvider.MoveTo<AuthorizationVM>(),
                canExecute: _ => CanMoveToAnotherPage
            );
        }
        #endregion Constructors

        #region Classes
        private class AuthData
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
        #endregion Classes

        #region Properties
        public string Login
        {
            get => _model.Login;
            set
            {
                _model.Login = value;
                OnPropertyChanged(propertyName: nameof(Login));
            }
        }

        public string Password
        {
            get => _model.Password;
            set
            {
                _model.Password = value;
                OnPropertyChanged(propertyName: nameof(Password));
            }
        }

        public string PasswordConfirmation
        {
            get => _model.PasswordConfirmation;
            set
            {
                _model.PasswordConfirmation = value;
                OnPropertyChanged(propertyName: nameof(PasswordConfirmation));
            }
        }

        public Command Registration => _registrationCommand.Value;

        public Command Back => _backCommand.Value;

        public RegistrationModule RegistrationModule { get; set; }
        #endregion Properties

        #region Methods
        private async Task SignUpAsync()
            => await RegistrationModule.SignUpAsync(login: Login, password: Password);
        #endregion Methods
    }
}
