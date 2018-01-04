using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataRelay.Grains.Interfaces;
using Metrics;
using Newtonsoft.Json;
using Orleans;
using Orleans.Concurrency;
using Orleans.Providers;
using Orleans.Runtime;

namespace DataRelay.Grains
{
	[StatelessWorker]
	[StorageProvider(ProviderName = "nonGuaranteedMessagesStore")]
	public class NonGuaranteedGrain : Grain<NonGuaranteedGrainState>, INonGuaranteedGrain, IRemindable
	{
		public async Task ReceiveData(string msg)
		{
			using (Metrics.Metric.Timer("Process Non-Guaranteed Request", Unit.Requests).NewContext())
			{
				var message = JsonConvert.DeserializeObject<Message>(msg);
				State.AddMessage(message);
				var reminder = await RegisterOrUpdateReminder(message.Id.ToString(), TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
			}
		}

		public async Task ReceiveReminder(string reminderName, TickStatus status)
		{
			using (Metrics.Metric.Timer("Process Reminder", Unit.Requests).NewContext())
			{
				var message = State.GetMessage(Guid.Parse(reminderName));
				if (message.PayloadType.Equals("opc", StringComparison.InvariantCultureIgnoreCase))
				{
					var forwarderGrain = GrainFactory.GetGrain<IForwarderGrain>(new Uri("https://requestb.in/1o2ycex1").ToString());
					var success = await forwarderGrain.Forward(JsonConvert.SerializeObject(message));
					if (success)
					{
						Metrics.Metric.Meter("Success", Unit.Errors).Mark();
					}
					else
					{
						Metrics.Metric.Meter("Error", Unit.Errors).Mark();
					}

					var reminder = await GetReminder(reminderName);
					await UnregisterReminder(reminder);
				}
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
