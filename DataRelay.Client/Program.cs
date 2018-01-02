using DataRelay.Grains.Interfaces;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime.Configuration;
using System;

namespace DataRelay.Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			var clientConfig = ClientConfiguration.LocalhostSilo();

			var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
			client.Connect().Wait();

			Console.WriteLine("Client connected.");

			// SMH: Specifying a key seems to be unnatural here...
			//var guaranteedGrain = client.GetGrain<IGuaranteedGrain>("guaranteed");
			//var msg = new Message("cms");
			//var jsonMsg = JsonConvert.SerializeObject(msg);
			//guaranteedGrain.ReceiveData(jsonMsg);
			
			// SMH: Specifying a key seems to be unnatural here...
			var guaranteedGrain = client.GetGrain<INonGuaranteedGrain>("nonGuaranteed");
			var msg = new Message("opc");
			var jsonMsg = JsonConvert.SerializeObject(msg);
			guaranteedGrain.ReceiveData(jsonMsg);

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
