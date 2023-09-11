using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using System;

namespace ElectronicJournal.ViewModels
{
	public class PasswordRecoveryVM : TrackedObject
	{
		#region Fields
		private readonly Lazy<Command> _backCommand;
		private readonly INavigationProvider _navigationProvider;
		#endregion Fields

		#region Constructor
		public PasswordRecoveryVM(INavigationProvider navigationProvider)
		{
			_navigationProvider = navigationProvider;
			_backCommand = Command.CreateLazyCommand(action: _ => _navigationProvider.MoveTo<AuthorizationVM>());
		}
		#endregion Constructor

		#region Properties
		public Command Back => _backCommand.Value;
		#endregion Properties
	}
}
