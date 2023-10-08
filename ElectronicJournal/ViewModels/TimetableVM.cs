using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using System;

namespace ElectronicJournal.ViewModels
{
	public class TimetableVM : VM
	{
		#region Fields
		private readonly INavigationProvider _navigationProvider;
		
		private readonly Lazy<Command> _backCommand;
		#endregion Fields

		#region Constructor
		public TimetableVM(INavigationProvider navigationProvider)
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
