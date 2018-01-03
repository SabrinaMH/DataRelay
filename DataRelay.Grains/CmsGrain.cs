using Orleans;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataRelay.Grains
{
	public class CmsGrain : Grain, IGuaranteedDataTypeGrain
	{
		private List<Uri> _urls = new List<Uri> { new Uri("https://requestb.in/1l9pkus1") };

		public async Task ReceiveData(string msg)
		{
			var forwarderTasks = new List<Task>(_urls.Count);
			foreach (var url in _urls)
			{
				var forwarderGrain = GrainFactory.GetGrain<IForwarderGrain>(url.ToString());
				forwarderTasks.Add(forwarderGrain.Forward(msg));
			}
			await Task.WhenAll(forwarderTasks);
		}
	}
}
