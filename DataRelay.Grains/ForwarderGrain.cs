using Orleans;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

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

		public async Task<HttpStatusCode> Forward(string msg)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, _url);
			var response = await _httpClient.SendAsync(request);
			return response.StatusCode;
		}
	}
}
