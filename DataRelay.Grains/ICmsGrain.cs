using Orleans;
using System.Threading.Tasks;

namespace DataRelay.Grains
{
	public interface ICmsGrain : IGrainWithStringKey
	{
		Task ReceiveData(string msg);
	}
}
