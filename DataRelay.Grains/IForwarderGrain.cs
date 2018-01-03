using Orleans;
using System.Net;
using System.Threading.Tasks;

namespace DataRelay.Grains
{
	public interface IForwarderGrain : IGrainWithStringKey
	{
		Task<HttpStatusCode> Forward(string request);
	}
}
