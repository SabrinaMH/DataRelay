using Orleans;
using System.Threading.Tasks;

namespace DataRelay.Grains.Interfaces
{
	public interface INonGuaranteedGrain : IGrainWithStringKey
	{
		Task ReceiveData(string msg);
	}
}
