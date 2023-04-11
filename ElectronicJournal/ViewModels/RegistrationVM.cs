using ElectronicJournal.Utilities;
using System;

namespace ElectronicJournal.ViewModels
{
	public class RegistrationVM : BaseVM
	{
		private readonly Lazy<Command> _backCommand;

		public RegistrationVM()
		{
			_backCommand = Command.CreateLazyCommand(action: obj => Navigation.Navigate<AuthorizationVM>());
		}

		public Command Back => _backCommand.Value;
	}
}
