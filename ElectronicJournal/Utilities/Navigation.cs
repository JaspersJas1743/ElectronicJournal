using ElectronicJournal.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicJournal.Utilities
{
	public static class Navigation
	{
		public static void Navigate<T>() where T : TrackedObject, new()
		{
			MainWindowVM mainWindowVM = Program.AppHost.Services.GetService<MainWindowVM>();
			mainWindowVM.Content = new T();
		}
	}
}
