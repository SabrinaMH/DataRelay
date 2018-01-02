using Orleans;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using DataRelay.Grains.Interfaces;

namespace DataRelay.Grains
{
	public class GuaranteedGrain : Grain, IGuaranteedGrain
	{
		public async Task ReceiveData(string msg)
		{
			var message = JsonConvert.DeserializeObject<Message>(msg);
			
			// SMH: See if anything can be done about this (eventually) long if else statement
			if (message.PayloadType.Equals("cms", StringComparison.InvariantCultureIgnoreCase))
			{
				var cmsGrain = GrainFactory.GetGrain<ICmsGrain>("cms");
				await cmsGrain.ReceiveData(msg);
			}

		}
	}
}
