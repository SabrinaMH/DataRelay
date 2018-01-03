using Orleans;
using Newtonsoft.Json;
using System.Threading.Tasks;
using DataRelay.Grains.Interfaces;

namespace DataRelay.Grains
{
	public class GuaranteedGrain : Grain, IGuaranteedGrain
	{
		// SMH: Problem: Has a chain of awaits, hence the previous link in the chain doesn't get an acknowledgement, before the message has been delivered to yet the next link in the chain.
		// Need to have persistence of some sort (perhaps in memory) to circumvent this. But in memory means data can be lost!
		public async Task ReceiveData(string msg)
		{
			var message = JsonConvert.DeserializeObject<Message>(msg);

			var guaranteedDataTypeGrain = GrainFactory.GetGrain<IGuaranteedDataTypeGrain>(message.PayloadType);
			await guaranteedDataTypeGrain.ReceiveData(msg);
		}
	}
}
