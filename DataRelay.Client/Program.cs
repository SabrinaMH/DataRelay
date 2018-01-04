using DataRelay.Grains.Interfaces;
using Metrics;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Threading.Tasks;

namespace DataRelay.Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			Metric.Config
				.WithHttpEndpoint("http://localhost:1234/client/")
				.WithAllCounters();

			var clientConfig = ClientConfiguration.LocalhostSilo();

			var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
			client.Connect().Wait();

			Console.WriteLine("Client connected.");

			// SMH: Specifying a key seems to be unnatural here...
			Task.Run(async () =>
			{
				var guaranteedGrain = client.GetGrain<IGuaranteedGrain>("guaranteed");
				var msg = new Message("cms");
				var jsonMsg = JsonConvert.SerializeObject(msg);
				while (true)
				{
					using (Metric.Timer("Guaranteed Request", Unit.Requests).NewContext())
					{
						await guaranteedGrain.ReceiveData(jsonMsg);
						await Task.Delay(1);
					}
				}
			}).Wait();

			// SMH: Specifying a key seems to be unnatural here...
			//var guaranteedGrain = client.GetGrain<INonGuaranteedGrain>("nonGuaranteed");
			//var msg = new Message("opc");
			//var jsonMsg = JsonConvert.SerializeObject(msg);
			//guaranteedGrain.ReceiveData(jsonMsg);

			Console.WriteLine("Press Enter to terminate...");
			Console.ReadLine();

			client.Close();
		}

		class Message
		{
			public string PayloadType { get; }

			public Message(string payloadType)
			{
				PayloadType = payloadType;
			}
		}
	}
}
