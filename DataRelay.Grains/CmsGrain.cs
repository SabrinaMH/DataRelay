using Orleans;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Metrics;
using System.Linq;

namespace DataRelay.Grains
{
	public class CmsGrain : Grain, IGuaranteedDataTypeGrain
	{
		private List<Uri> _urls = new List<Uri> { new Uri("https://requestb.in/1o2ycex1") };

		public async Task ReceiveData(string msg)
		{
			var forwarderTasks = new List<Task<bool>>(_urls.Count);
			foreach (var url in _urls)
			{
				var forwarderGrain = GrainFactory.GetGrain<IForwarderGrain>(url.ToString());
				forwarderTasks.Add(forwarderGrain.Forward(msg));
			}
			bool[] results = await Task.WhenAll(forwarderTasks);
			var errors = Metric.Meter("Error", Unit.Errors);
			var successes = Metric.Meter("Success", Unit.Errors);
			successes.Mark(results.Count(x => x == true));
			errors.Mark(results.Count(x => x == false));
		}
	}
}
