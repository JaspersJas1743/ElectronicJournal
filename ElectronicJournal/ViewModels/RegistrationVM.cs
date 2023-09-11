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
		private readonly INavigationProvider _navigationProvider;

		private readonly Lazy<Command> _backCommand;
		private readonly Lazy<Command> _registrationCommand;

		private string _code;
		#endregion Fields

		#region Constructors
		public RegistrationVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;
			
			DefaultButtonContent = "Зарегистрироваться";
			_buttonContent = DefaultButtonContent;

			_backCommand = Command.CreateLazyCommand(
				action: _ => _navigationProvider.MoveTo<AuthorizationVM>(),
				canExecute: _ => ButtonContent.Equals(DefaultButtonContent)
			);

			_registrationCommand = Command.CreateLazyCommand(action: async _ => 
			{
				if (await TryExecuteTask(taskForExecute: TryCheckRegistrationCode))
					_navigationProvider.MoveTo<RegistrationOfAuthorizationDataVM>();
				else
					MessageWindow.ShowError(text: "Проверьте правильность кода или обратитесь в Вашу организацию");
			}, 
			canExecute: _ => Code?.Length == CodeEntryPanel.MaxCountOfCell && ButtonContent.Equals(DefaultButtonContent));
		}
		#endregion Constructors

		#region Properties
		public Command Registration => _registrationCommand.Value;

		public Command Back => _backCommand.Value;

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
		private async Task<bool> TryCheckRegistrationCode()
		{
			try
			{
				int userId = await ApiClient.GetAsync<int>(apiMethod: "User/GetUserIdByRegistrationCode", arg: Code);
				ApiClient.SetIdForUser(id: userId);
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
