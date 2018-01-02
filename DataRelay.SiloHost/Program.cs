using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime.Configuration;

namespace DataRelay.SiloHost
{
	public class Program
	{
		static void Main(string[] args)
		{
			var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
			siloConfig.UseStartupType<Startup>();
			siloConfig.
			var silo = new Orleans.Runtime.Host.SiloHost("DataRelaySilo", siloConfig);
			silo.InitializeOrleansSilo();
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
