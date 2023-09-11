using ElectronicJournal.Models.ForPage;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Api;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
	public class RegistrationOfAuthorizationDataVM : TrackedObject
	{
		#region Fields
		private readonly INavigationProvider _navigationProvider;

		private readonly Lazy<Command> _registrationCommand;
		private readonly Lazy<Command> _backCommand;

		private RegistrationOfAuthorizationDataModel _model;
		#endregion Fields

		#region Constructors
		public RegistrationOfAuthorizationDataVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;

			DefaultButtonContent = "Зарегистрироваться";
			_buttonContent = DefaultButtonContent;
			_model = new RegistrationOfAuthorizationDataModel();

			_registrationCommand = Command.CreateLazyCommand(action: async _ =>
			{
				if (await TryExecuteTask(taskForExecute: TryRegistrationDataAsync))
				{
					AuthorizationVM avm = Program.AppHost.Services.GetService<AuthorizationVM>();
					avm.Login = Login;
					avm.Password = Password;
					_navigationProvider.MoveTo<AuthorizationVM>();
				}
				else
					MessageWindow.ShowError(text: "Введенные учетные данные уже заняты! :(");
			}, canExecute: _ => _model.IsValid && ButtonContent.Equals(DefaultButtonContent));

			_backCommand = Command.CreateLazyCommand(
				action: _ => _navigationProvider.MoveTo<AuthorizationVM>(),
				canExecute: _ => ButtonContent.Equals(DefaultButtonContent)
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
		#endregion Properties

		#region Methods
		private async Task<bool> TryRegistrationDataAsync()
		{
			bool availabilityForLogin = await ApiClient.GetAsync<bool>(apiMethod: "User/GetAvailabilityForLogin", arg: Login);
			if (availabilityForLogin)
				await ApiClient.PostAsync(apiMethod: "User/Registration", arg: new AuthData() { Login = Login, Password = Password });
			return availabilityForLogin;
		}
		#endregion Methods
	}
}
