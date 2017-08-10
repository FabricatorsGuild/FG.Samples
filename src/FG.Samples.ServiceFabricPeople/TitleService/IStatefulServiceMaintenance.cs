using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace TitleService
{
	public interface IStatefulServiceMaintenance : IService
	{
		Task<string[]> GetStatesAsync();
	}
}