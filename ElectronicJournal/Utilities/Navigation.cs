using ElectronicJournal.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicJournal.Utilities
{
	public static class Navigation
	{
		public static void Navigate<T>() where T : BaseVM
		{
			MainWindowVM mainWindowVM = Program.AppHost.Services.GetService<MainWindowVM>();
			mainWindowVM.Content = Program.AppHost.Services.GetService<T>();
		}
	}
}
