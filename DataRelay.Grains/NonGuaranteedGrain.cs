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
	public class NonGuaranteedGrain : Grain<NonGuaranteedGrainState>, INonGuaranteedGrain, IRemindable
	{
		public async Task ReceiveData(string msg)
		{
			var message = JsonConvert.DeserializeObject<Message>(msg);
			State.AddMessage(message);
			var reminder = RegisterOrUpdateReminder(message.Id.ToString(), TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
		}

		public async Task ReceiveReminder(string reminderName, TickStatus status)
		{
			var message = State.GetMessage(Guid.Parse(reminderName));
			
			if (message.PayloadType.Equals("opc", StringComparison.InvariantCultureIgnoreCase))
			{
				var forwarderGrain = GrainFactory.GetGrain<IForwarderGrain>(new Uri("https://requestb.in/1l9pkus2").ToString());
				var response = await forwarderGrain.Forward(JsonConvert.SerializeObject(message));
				
				var reminder = await GetReminder(reminderName);
				await UnregisterReminder(reminder);
			}
		}
	}

	public class NonGuaranteedGrainState
	{
		private Dictionary<Guid, Message> _messages { get; }

		public NonGuaranteedGrainState()
		{
			_messages = new Dictionary<Guid, Message>();
		}

		public void AddMessage(Message message)
		{
			_messages.Add(message.Id, message);
		}

		public Message GetMessage(Guid id)
		{
			return _messages[id];
		}
	}
}
