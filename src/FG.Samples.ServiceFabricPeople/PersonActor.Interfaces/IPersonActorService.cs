using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Remoting;

namespace PersonActor.Interfaces
{
	public interface IPersonActorService : IService
	{
		Task<IDictionary<string, Person>> GetPersons(CancellationToken cancellationToken);

		Task<string> CreatePerson(string name, string title, CancellationToken cancellationToken);
	}

	public interface IActorServiceMaintenance : IService
	{
		Task<ActorId[]> GetActors(CancellationToken cancellationToken);

		Task<State[]> GetStates(ActorId actorId, CancellationToken cancellationToken);
	}

	[DataContract]
	public class State
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string TypeName { get; set; }
		[DataMember]
		public string Data { get; set; }
	}
}