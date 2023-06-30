using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ElectronicJournal
{
	public class Program
	{
		public static IHost AppHost { get; set; }

		[STAThread]
		public static void Main(string[] args)
		{
			AppHost = Host.CreateDefaultBuilder()
				.ConfigureServices(configureDelegate: services =>
				{
					services.AddSingleton<App>();
					services.AddSingleton<MainWindow>();
					services.AddSingleton<MainWindowVM>();
					services.AddSingleton<AuthorizationVM>();
					services.AddSingleton<RegistrationVM>();
					services.AddSingleton<TimetableVM>();
					services.AddSingleton<PasswordRecoveryVM>();
					
					services.AddScoped<IConfigProvider, ConfigurationProvider>();
					services.AddScoped<INavigationProvider, NavigationProvider>();
				}).Build();
			App app = AppHost.Services.GetService<App>();
			app.Run();
		}
	}
}
