using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataRelay.Grains.Interfaces;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;

namespace DataRelay.Grains
{
	[StorageProvider(ProviderName = "nonGuaranteedMessagesStore")]
	public class NonGuaranteedGrain<NonGaranteedGrainState> : Grain<NonGuaranteedGrainState>, INonGuaranteedGrain, IRemindable, IPer
	{
		public async Task ReceiveData(string msg)
		{
			var reminder = RegisterOrUpdateReminder("sendMessage", TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
		
			var message = JsonConvert.DeserializeObject<Message>(msg);

			{
			
			}
		}

		public async Task ReceiveReminder(string reminderName, TickStatus status)
		{
			if (reminderName == "sendMessage")
			{
				// SMH: See if anything can be done about this (eventually) long if else statement
				if (message.PayloadType.Equals("opc", StringComparison.InvariantCultureIgnoreCase))
				{
					var forwarderGrain = GrainFactory.GetGrain<IForwarderGrain>(new Uri("https://requestb.in/1l9pkus2").ToString());

					var reminder = await GetReminder("sendMessage");
					await UnregisterReminder(reminder);
				}
			}
		}
	}

	public class NonGuaranteedGrainState
	{
		public List<string> Messages { get; }

		public void AddMessage(string message)
		{
			Messages.Add(message);
		}
	}
}
