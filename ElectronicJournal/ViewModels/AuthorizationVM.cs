using ElectronicJournal.Models;
using ElectronicJournal.Utilities;
using System;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class AuthorizationVM : TrackedObject
	{
		#region Fields
		private AuthorizationModel _model;
		private readonly Lazy<Command> _authorize;
		private readonly Lazy<Command> _moveToRegistration;
		private readonly Lazy<Command> _moveToPasswordRecovery;
		#endregion Fields

		#region Constructor
		public AuthorizationVM()
		{
			_model = new AuthorizationModel();
			_authorize = Command.CreateLazyCommand(action: obj =>
			{
				MessageBox.Show($"Login={Login};Password={Password};" +
				$"\nToken={JWT.Generate(dataForGeneration: String.Concat(arg0: Login, arg1: Password))}");
			});
			_moveToRegistration = Command.CreateLazyCommand(action: obj => Navigation.Navigate<RegistrationVM>());
			_moveToPasswordRecovery = Command.CreateLazyCommand(action: obj => Navigation.Navigate<PasswordRecoveryVM>());
		}
		#endregion Constructor

		#region Properties
		public string Login
		{
			get => _model.Login;
			set
			{
				_model.Login = value;
				OnPropertyChanged(nameof(Login));
			}
		}

		public string Password
		{
			get => _model.Password;
			set
			{
				_model.Password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		public Command Authorize => _authorize.Value;
		public Command MoveToRegistration => _moveToRegistration.Value;
		public Command MoveToPasswordRecovery => _moveToPasswordRecovery.Value;
		public string StudentsImage => $"pack://application:,,,/Resources/Images/{Theme.CurrentTheme}/students.svg";
		#endregion Properties
	}
}
