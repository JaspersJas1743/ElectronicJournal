using ElectronicJournal.Resources.CustomElements;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Api;
using ElectronicJournal.Utilities.Api.Models;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using System;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
	public class RegistrationVM : TrackedObject
	{
		#region Fields
		private const string DefaultButtonContent = "Зарегистрироваться";

		private readonly INavigationProvider _navigationProvider;

		private readonly Lazy<Command> _backCommand;
		private readonly Lazy<Command> _registrationCommand;
		private string _buttonContent;
		private string _code;
		#endregion Fields

		#region Constructors
		public RegistrationVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;
			_backCommand = Command.CreateLazyCommand(action: obj => _navigationProvider.MoveTo<AuthorizationVM>());
			_buttonContent = DefaultButtonContent;
			_registrationCommand = Command.CreateLazyCommand(action: async obj =>
			{
				// TODO: Запрос к апи на проверку кода регистрации (имеется ли такой)
				// Если да, то установить айди челика в ApiClient и перейти на страницу
				// создания логина и пароля
				//Если нет, соответственно, ошибку выдать
				var checkRegistrationCodeTask = TryCheckRegistrationCode();

				int count = 0;
				while (!checkRegistrationCodeTask.IsCompleted)
				{
					if (count == 3)
						count = 0;

					ButtonContent = "Загрузка" + new String(c: '.', count: ++count);

					await Task.Delay(millisecondsDelay: 250);
				}

				(bool result, string message) = await checkRegistrationCodeTask;
				ButtonContent = DefaultButtonContent;
				if (result)
				{
					MessageWindow.ShowInformation(text: message);
					return;
				}
				MessageWindow.ShowError(text: message);
			}, canExecute: obj => Code?.Length == CodeEntryPanel.MaxCountOfCell && ButtonContent.Equals(DefaultButtonContent));
		}
		#endregion Constructors

		#region Properties
		public Command Registration => _registrationCommand.Value;

		public Command Back => _backCommand.Value;

		public string ButtonContent
		{
			get => _buttonContent;
			set
			{
				_buttonContent = value;
				OnPropertyChanged(propertyName: nameof(ButtonContent));
			}
		}

		public string Code
		{
			get => _code;
			set
			{
				_code = value;
				OnPropertyChanged(propertyName: nameof(Code));
			}
		}
		#endregion Properties

		#region Methods
		private async Task<(bool Result, string Message)> TryCheckRegistrationCode()
		{
			try
			{
				TokenResponse tokenResponse = await ApiClient.SendAndGetAsync<TokenResponse>(apiMethod: "User/CheckRegistrationCode", arg: Code);
				ApiClient.SetIdForUser(id: tokenResponse.Id);
				return (Result: true, Message: $"Переход на страницу установки логина и пароля для аккаунта пользователя №{tokenResponse.Id}");
			}
			catch (Exception ex)
			{
				return (Result: false, Message: ex.Message);
			}
		}

		#endregion Methods
	}
}
