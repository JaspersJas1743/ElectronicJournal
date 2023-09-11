using ElectronicJournal.ViewModels.Tools;

namespace ElectronicJournal.Utilities.Navigation
{
	public interface INavigationProvider
	{
		void MoveTo<NewPage>() where NewPage : TrackedObject;
	}
}
