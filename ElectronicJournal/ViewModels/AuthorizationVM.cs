using ElectronicJournal.Models;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.Utilities.Validator;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class AuthorizationVM : VM
    {
        #region Fields
        private readonly INavigationProvider _navigationProvider;

        private readonly Lazy<Command> _authorize;
        private readonly Lazy<Command> _moveToRegistration;
        private readonly Lazy<Command> _moveToPasswordRecovery;

        private readonly IValidator<AuthorizationModel> _authorizationModelValidator;
        private AuthorizationModel _model;
        #endregion Fields

        #region Constructor
        public AuthorizationVM(INavigationProvider navigationProvider, IValidator<AuthorizationModel> validator)
            : base(defaultButtonContent: "Войти")
        {
            _navigationProvider = navigationProvider;
            _model = new AuthorizationModel();
            _authorizationModelValidator = validator;

            _authorize = Command.CreateLazyCommand(action: async _ =>
            {
                try
                {
                    await ExecuteTask(taskForExecute: SignInAsync);
                    _navigationProvider.MoveTo<TimetableVM>();
                } catch (Exception ex) 
                {
                    MessageWindow.ShowError(text: ex.Message);
                }
            },
            canExecute: _ => _authorizationModelValidator.Validate(instance: _model).IsValid && CanMoveToAnotherPage);

            _moveToRegistration = Command.CreateLazyCommand(
                action: _ => _navigationProvider.MoveTo<RegistrationVM>(),
                canExecute: _ => CanMoveToAnotherPage
            );

            _moveToPasswordRecovery = Command.CreateLazyCommand(
                action: _ => _navigationProvider.MoveTo<PasswordRecoveryVM>(),
                canExecute: _ => CanMoveToAnotherPage
            );
        }
        #endregion Constructor

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

        public Command Authorize => _authorize.Value;
        public Command MoveToRegistration => _moveToRegistration.Value;
        public Command MoveToPasswordRecovery => _moveToPasswordRecovery.Value;
        #endregion Properties

        #region Methods
        private async Task SignInAsync()
        {
            ElectronicJournalApi api = new ElectronicJournalApi(login: Login, password: Password);
            await api.SignInAsync();
        }
        #endregion Methods
    }
}
