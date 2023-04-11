using ElectronicJournal.Utilities;
using System;

namespace ElectronicJournal.ViewModels
{
	public class PasswordRecoveryVM : BaseVM
	{
		private readonly Lazy<Command> _backCommand;

		public PasswordRecoveryVM()
		{
			_backCommand = Command.CreateLazyCommand(action: obj => Navigation.Navigate<AuthorizationVM>());
		}

		public Command Back => _backCommand.Value;
	}
}
