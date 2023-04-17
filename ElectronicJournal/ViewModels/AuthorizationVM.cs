using ElectronicJournal.Models;
using ElectronicJournal.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
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
			_authorize = Command.CreateLazyCommand(action: async obj =>
			{
				//User/GetIfExist?login={Login}&password={Password}";
				User user;
				try
				{
					user = await ApiClient.SendAsync<User>(apiMethod: "User/GetIfExist", args: new Dictionary<string, string>()
					{
						["login"] = Login,
						["password"] = Password
					});
				} catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}

				MessageBox.Show($"Добро пожаловать, {user.Name} {user.Patronymic}");
				Navigation.Navigate<TimetableVM>();
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
		#endregion Properties
	}
}
