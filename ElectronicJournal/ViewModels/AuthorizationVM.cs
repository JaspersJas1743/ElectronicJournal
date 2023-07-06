using ElectronicJournal.Models;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Api;
using ElectronicJournal.Utilities.Api.Models;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.Utilities.Validator;
using ElectronicJournal.ViewModels.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
	public class AuthorizationVM : TrackedObject
	{
		#region Fields
		private const string DefaultButtonContent = "Войти";

		private readonly INavigationProvider _navigationProvider;

		private AuthorizationModel _model;
		private string _buttonContent;
		private readonly Lazy<Command> _authorize;
		private readonly Lazy<Command> _moveToRegistration;
		private readonly Lazy<Command> _moveToPasswordRecovery;
		#endregion Fields

		#region Constructor
		public AuthorizationVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;
			_model = new AuthorizationModel();
			_buttonContent = DefaultButtonContent;
			_authorize = Command.CreateLazyCommand(action: async obj =>
			{
				var authTask = TryAuth();

				int count = 0;
				while (!authTask.IsCompleted)
				{
					if (count == 3)
						count = 0;
					ButtonContent = "Загрузка" + new String(c: '.', count: ++count);

					await Task.Delay(millisecondsDelay: 250);
				}

				(bool result, string message) = await authTask;
				ButtonContent = DefaultButtonContent;
				if (result)
				{
					_navigationProvider.MoveTo<TimetableVM>();
					return;
				}
				MessageWindow.ShowError(text: message);
			}, canExecute: obj => _model.IsValid && ButtonContent.Equals(DefaultButtonContent));
			_moveToRegistration = Command.CreateLazyCommand(action: obj => _navigationProvider.MoveTo<RegistrationVM>());
			_moveToPasswordRecovery = Command.CreateLazyCommand(action: obj => _navigationProvider.MoveTo<PasswordRecoveryVM>());
		}
		#endregion Constructor

		#region Properties
		public string Login
		{
			get => _model.Login;
			set
			{
				_model.Login = value;
				OnModelPropertyChanged(propertyName: nameof(Login));
			}
		}

		public string Password
		{
			get => _model.Password;
			set
			{
				_model.Password = value;
				OnModelPropertyChanged(propertyName: nameof(Password));
			}
		}

		public bool ModelIsValid => _model.IsValid && ButtonContent.Equals("Войти");

		public string ButtonContent
		{
			get => _buttonContent;
			set
			{
				_buttonContent = value;
				OnPropertyChanged(propertyName: nameof(ButtonContent));
			}
		}

		public Command Authorize => _authorize.Value;
		public Command MoveToRegistration => _moveToRegistration.Value;
		public Command MoveToPasswordRecovery => _moveToPasswordRecovery.Value;
		#endregion Properties

		#region Methods
		private void OnModelPropertyChanged([CallerMemberName] string propertyName = "")
		{
			OnPropertyChanged(propertyName: propertyName);
			OnPropertyChanged(propertyName: nameof(ModelIsValid));
		}

		private async Task<(bool Result, string Message)> TryAuth()
		{
			List<ValidationResult> errorMessages = new List<ValidationResult>();
			if (!ObjectValidator.Validate(instance: _model, messages: out errorMessages))
				return (Result: false, Message: String.Join(separator: "\n", values: errorMessages));

			try
			{
				TokenResponse token = await ApiClient.SendAndGetAsync<TokenResponse>(apiMethod: "User/GetToken", arg: Login);
				ApiClient.SetTokenForAuthorization(token: token.Token);
				ApiClient.SetIdForUser(id: token.Id);
				User user = await ApiClient.SendAndGetAsync<User>(apiMethod: "User/GetUserIfPasswordExist", arg: Password);
				return (Result: true, Message: $"Добро пожаловать, {user.Surname} {user.Name} {user.Patronymic}");
			}
			catch (Exception ex)
			{
				return (Result: false, Message: ex.Message);
			}
		}
		#endregion Methods
	}
}
