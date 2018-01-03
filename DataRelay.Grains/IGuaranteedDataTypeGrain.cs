using Orleans;
using System.Threading.Tasks;

namespace DataRelay.Grains
{
	public interface IGuaranteedDataTypeGrain : IGrainWithStringKey
	{
		Task ReceiveData(string msg);
	}
}
