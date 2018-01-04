using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime.Configuration;
using Orleans.Storage;
using OrleansDashboard;

namespace DataRelay.SiloHost
{
	public class Program
	{
		static void Main(string[] args)
		{

			var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
			siloConfig.UseStartupType<Startup>();
			siloConfig.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.ReminderTableGrain;
			siloConfig.Globals.RegisterStorageProvider<MemoryStorage>("nonGuaranteedMessagesStore");
			var silo = new Orleans.Runtime.Host.SiloHost("DataRelaySilo", siloConfig);
			silo.InitializeOrleansSilo();
			silo.Config.Globals.RegisterDashboard();
			silo.StartOrleansSilo();

			Console.WriteLine("Silo started.");
			Console.ReadKey();

			silo.ShutdownOrleansSilo();
		}
	}

	public class Startup
	{
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<HttpClient, HttpClient>();
			return services.BuildServiceProvider();
		}
	}
}
