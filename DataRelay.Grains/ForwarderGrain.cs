using Orleans;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Metrics;

namespace DataRelay.Grains
{
	public class ForwarderGrain : Grain, IForwarderGrain
	{
		private readonly HttpClient _httpClient;
		private Uri _url;

		public ForwarderGrain(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public override Task OnActivateAsync()
		{
			_url = new Uri(this.GetPrimaryKeyString());
			return base.OnActivateAsync();
		}

		public async Task<bool> Forward(string msg)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, _url);
			using (Metric.Timer("Forward Request", Unit.Requests).NewContext())
			{
				var response = await _httpClient.SendAsync(request);
				return response.IsSuccessStatusCode;
			}
		}
	}
}
