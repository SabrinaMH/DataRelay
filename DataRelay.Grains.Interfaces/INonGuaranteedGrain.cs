using Orleans;
using System.Threading.Tasks;

namespace DataRelay.Grains.Interfaces
{
	public interface INonGuaranteedGrain : IGrainWithIntegerKey
	{
		Task ReceiveData(string msg);
	}
}
