using ElectronicJournal.Models.ForAPI;
using ElectronicJournal.Models.ForPage;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Api;
using ElectronicJournal.Utilities.Api.Models;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using System;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
	public class AuthorizationVM : TrackedObject
	{
		#region Fields
		private readonly INavigationProvider _navigationProvider;

		private readonly Lazy<Command> _authorize;
		private readonly Lazy<Command> _moveToRegistration;
		private readonly Lazy<Command> _moveToPasswordRecovery;

		private AuthorizationModel _model;
		#endregion Fields

		#region Constructor
		public AuthorizationVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;

			DefaultButtonContent = "Войти";
			_buttonContent = DefaultButtonContent;
			_model = new AuthorizationModel();

			_authorize = Command.CreateLazyCommand(action: async _ =>
			{
				if (await TryExecuteTask(taskForExecute: TryAuth))
					_navigationProvider.MoveTo<TimetableVM>();
				else
					MessageWindow.ShowError(text: "Неправильный логин и/или пароль");
			},
			canExecute: _ => _model.IsValid && ButtonContent.Equals(DefaultButtonContent));

			_moveToRegistration = Command.CreateLazyCommand(
				action: _ => _navigationProvider.MoveTo<RegistrationVM>(),
				canExecute: _ => ButtonContent.Equals(DefaultButtonContent)
			);

			_moveToPasswordRecovery = Command.CreateLazyCommand(
				action: _ => _navigationProvider.MoveTo<PasswordRecoveryVM>(),
				canExecute: _ => ButtonContent.Equals(DefaultButtonContent)
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
		private async Task<bool> TryAuth()
		{
			try
			{
				TokenResponse response = await ApiClient.GetAsync<TokenResponse>(apiMethod: "User/GetToken", arg: Login);
				ApiClient.SetTokenForAuthorization(token: response.Token);
				ApiClient.SetIdForUser(id: response.Id);
				_ = await ApiClient.GetAsync<User>(apiMethod: "User/GetUserIfPasswordExist", arg: Password);
				return true;
			}
			catch 
			{ 
				return false; 
			}
		}
		#endregion Methods
	}
}
