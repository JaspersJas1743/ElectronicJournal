using ElectronicJournal.Utilities;
using System;

namespace ElectronicJournal.ViewModels
{
	public class TimetableVM : TrackedObject
	{
		private readonly Lazy<Command> _backCommand;

		public TimetableVM()
		{
			_backCommand = Command.CreateLazyCommand(action: obj => Navigation.Navigate<AuthorizationVM>());
		}

		public Command Back => _backCommand.Value;
	}
}
