using Orleans;
using System.Threading.Tasks;

namespace DataRelay.Grains
{
	public interface IForwarderGrain : IGrainWithStringKey
	{
		Task Forward(string request);
	}
}
