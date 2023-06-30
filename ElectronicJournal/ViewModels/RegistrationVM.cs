using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using System;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class RegistrationVM : TrackedObject
	{
		private readonly INavigationProvider _navigationProvider;

		private readonly Lazy<Command> _backCommand;
		private readonly Lazy<Command> _registrationCommand;
		private string _code;

		public RegistrationVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;
			_backCommand = Command.CreateLazyCommand(action: obj => _navigationProvider.MoveTo<AuthorizationVM>());
			_registrationCommand = Command.CreateLazyCommand(action: obj => MessageBox.Show(""));
		}

		public Command Registration => _registrationCommand.Value;

		public Command Back => _backCommand.Value;

// TODO: Добавить биндинг кода из панели, чтобы он отображался
		public string Code
		{
			get => _code;
			set
			{
				_code = value;
				OnPropertyChanged(propertyName: nameof(Code));
			}
		}
	}
}
