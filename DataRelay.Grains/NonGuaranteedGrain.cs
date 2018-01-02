using System;
using System.Threading.Tasks;
using DataRelay.Grains.Interfaces;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime;

namespace DataRelay.Grains
{
	public class NonGuaranteedGrain : Grain, INonGuaranteedGrain, IRemindable
	{
		public async Task ReceiveData(string msg)
		{
			var reminder = await RegisterOrUpdateReminder("sendMessage", TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));




			var message = JsonConvert.DeserializeObject<Message>(msg);

			// SMH: See if anything can be done about this (eventually) long if else statement
			if (message.PayloadType.Equals("opc", StringComparison.InvariantCultureIgnoreCase))
			{
			
			}
		}

		public async Task ReceiveReminder(string reminderName, TickStatus status)
		{
			if (reminderName == "sendMessage")
			{
				// Send http request
				var reminder = await GetReminder("sendMessage");
				await UnregisterReminder(reminder);
			}
		}
	}
}
