using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using System;

namespace ElectronicJournal.ViewModels
{
	public class PasswordRecoveryVM : TrackedObject
	{
		private readonly Lazy<Command> _backCommand;
		private readonly INavigationProvider _navigationProvider;

		public PasswordRecoveryVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;
			_backCommand = Command.CreateLazyCommand(action: obj => _navigationProvider.MoveTo<AuthorizationVM>());
		}

		public Command Back => _backCommand.Value;
	}
}
