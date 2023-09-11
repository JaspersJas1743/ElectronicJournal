using ElectronicJournal.ViewModels;
using ElectronicJournal.ViewModels.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicJournal.Utilities.Navigation
{
	public class NavigationProvider : INavigationProvider
	{
		public void MoveTo<NewPage>() where NewPage : TrackedObject
		{
			MainWindowVM contentDrawable = Program.AppHost.Services.GetService<MainWindowVM>();
			contentDrawable.Content = Program.AppHost.Services.GetService<NewPage>();
		}
	}
}
