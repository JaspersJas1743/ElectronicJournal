using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ElectronicJournal
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder()
				.ConfigureServices(configureDelegate: services => services.AddSingleton<App>()).Build();
			App app = host.Services.GetService<App>();
            app.Run();
		}
	}
}
