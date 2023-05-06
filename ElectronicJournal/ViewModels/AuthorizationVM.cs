using ElectronicJournal.Models;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class AuthorizationVM : TrackedObject
	{
		#region Fields
		private AuthorizationModel _model;
		private string _buttonContent;
		private readonly Lazy<Command> _authorize;
		private readonly Lazy<Command> _moveToRegistration;
		private readonly Lazy<Command> _moveToPasswordRecovery;
		#endregion Fields

		#region Constructor
		public AuthorizationVM()
		{
			_model = new AuthorizationModel(login: String.Empty, password: String.Empty);
			_buttonContent = "Войти";
			_authorize = Command.CreateLazyCommand(action: async obj =>
			{
				var authTask = TryAuth();
				
				int count = 0;
				while (!authTask.IsCompleted)
				{
					if (count == 4)
						count = 0;

					await Task.Delay(millisecondsDelay: 250);

					ButtonContent = "Загрузка" + new String(c: '.', count: count);
					++count;
				}

				(bool result, string message) = await authTask;
				ButtonContent = "Войти";
				if (result)
				{
					MessageWindow.ShowMessage(text: message);
					Navigation.Navigate<TimetableVM>();
				}
				else MessageWindow.ShowError(text: message);

			});
			_moveToRegistration = Command.CreateLazyCommand(action: obj => MessageWindow.ShowMessage(text: "Переход на страницу регистрации"));
			_moveToPasswordRecovery = Command.CreateLazyCommand(action: obj => MessageWindow.ShowInformation(text: "Переход на страницу восстановления пароля"));
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

		public bool LoginIsValid => _model.LoginIsValid;

		public bool PasswordIsValid => _model.PasswordIsValid;

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
			OnPropertyChanged(propertyName: propertyName + "IsValid");
			OnPropertyChanged(propertyName: nameof(ModelIsValid));
		}

		private async Task<(bool Result, string Message)> TryAuth()
		{
			List<ValidationResult> errorMessages = new List<ValidationResult>();
			if (!ObjectValidator.Validate(instance: _model, messages: out errorMessages))
				return (Result: false, Message: String.Join(separator: "\n", values: errorMessages));

			try
			{
				string token = await ApiClient.SendAndGetStringAsync(apiMethod: "User/GetToken", arg: Login);
				ApiClient.SetTokenForAuthorization(token: token);
				User user = await ApiClient.SendAndGetAsync<User>(apiMethod: "User/CheckPassword", arg: Password);
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
