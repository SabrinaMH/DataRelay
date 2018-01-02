using Orleans;
using System.Threading.Tasks;

namespace DataRelay.Grains.Interfaces
{
	public interface IGuaranteedGrain : IGrainWithStringKey
	{
		Task ReceiveData(string msg);
	}
}
