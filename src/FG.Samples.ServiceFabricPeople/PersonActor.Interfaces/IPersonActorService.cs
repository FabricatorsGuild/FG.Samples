using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace PersonActor.Interfaces
{
	public interface IPersonActorService : IService
	{
		Task<IDictionary<string, Person>> GetPersons(CancellationToken cancellationToken);
	}
}