using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace PersonActor.Interfaces
{
	public interface IPersonActor : IActor
	{
		Task<Person> GetPersonAsync(CancellationToken cancellationToken);
		Task SetTitleAsync(string title, CancellationToken cancellationToken);
	}
}
