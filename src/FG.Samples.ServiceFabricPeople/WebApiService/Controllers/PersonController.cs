using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Services.Remoting.Runtime.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using PersonActor.Interfaces;

namespace WebApiService.Controllers
{


	[Route("api/[controller]")]
	public class PersonController : ControllerBase, ILoggableController
    {
	    private readonly Func<IPartitionEnumerationManager> _partitionEnumerationManagerFactory;
	    private readonly object _lock = new object();

        private static PartitionHelper _partitionHelper;

        public PersonController(StatelessServiceContext context, Func<IPartitionEnumerationManager> partitionEnumerationManagerFactory) : base(context)
        {
	        _partitionEnumerationManagerFactory = partitionEnumerationManagerFactory;
        }

        private PartitionHelper GetOrCreatePartitionHelper()
        {
            if (_partitionHelper != null)
            {
                return _partitionHelper;
            }

            lock (_lock)
            {
                if (_partitionHelper == null)
                {
                    _partitionHelper = new PartitionHelper(_partitionEnumerationManagerFactory);
                }
                return _partitionHelper;
            }
        }

		[HttpGet]
		// GET api/person 
		public async Task<IDictionary<string, IDictionary<string, Person>>> Get()
		{
            var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/PersonActorService");
            var allPersons = new Dictionary<string, IDictionary<string, Person>>();

            var partitionKeys = await GetOrCreatePartitionHelper().GetInt64Partitions(serviceUri, ServicesCommunicationLogger);
			var getAllPersonsTasks = new List<Task<IDictionary<string, Person>>>();
			var matchTaskToPartition = new Dictionary<int, string>();
            foreach (var partitionKey in partitionKeys)
            {
                var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);
                var proxy = actorProxyFactory.CreateActorServiceProxy<IPersonActorService>(
                    serviceUri,
                    partitionKey.LowKey);

	            var partitionInfo = $"{partitionKey.LowKey} - {partitionKey.HighKey}";
	            var getPersonsTask = proxy.GetPersons(CancellationToken.None);
	            getAllPersonsTasks.Add(getPersonsTask);
	            matchTaskToPartition.Add(getPersonsTask.Id, partitionInfo);
            }

			await Task.WhenAll(getAllPersonsTasks);

			foreach (var getAllPersonsTask in getAllPersonsTasks)
			{
				var partitionInfo = matchTaskToPartition[getAllPersonsTask.Id];
				var result = await getAllPersonsTask;
				allPersons.Add(partitionInfo, result);
			}

			return allPersons;
		}

		[HttpGet("{id}")]
		// GET api/person/ardinheli 
		public async Task<Person> Get(string id)
		{
			var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/PersonActorService");
			var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);

			var proxy = actorProxyFactory.CreateActorProxy<IPersonActor>(
				serviceUri,
				new ActorId(id));

			var person = await proxy.GetPersonAsync(CancellationToken.None);

			return person;

		}



	    [HttpGet("{id}/operation/{operation}/{payload}")]
	    // GET api/person/ardinheli
	    public async Task<Person> Invoke(string id, string operation, string payload)
	    {
		    var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/PersonActorService");

		    FG.ServiceFabric.Actors.Client.ActorProxyFactory actorProxyFactory = null;
		    IPersonActor actorProxy = null;
		    if ("create".Equals(operation, StringComparison.InvariantCultureIgnoreCase))
		    {
			    var title = payload;

			    var serviceProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);
			    var serviceProxy = serviceProxyFactory.CreateActorServiceProxy<IPersonActorService>(
				    serviceUri,
				    new ActorId(id));

			    await serviceProxy.CreatePerson(id, title, CancellationToken.None);
		    }
		    else if ("settitle".Equals(operation, StringComparison.InvariantCultureIgnoreCase))
		    {
			    var title = payload;

			    actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);

			    actorProxy = actorProxyFactory.CreateActorProxy<IPersonActor>(
				    serviceUri,
				    new ActorId(id));

			    await actorProxy.SetTitleAsync(title, CancellationToken.None);
		    }

		    actorProxyFactory = actorProxyFactory ?? new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);

		    actorProxy = actorProxy ?? actorProxyFactory.CreateActorProxy<IPersonActor>(
			                 serviceUri,
			                 new ActorId(id));

		    var person = await actorProxy.GetPersonAsync(CancellationToken.None);

		    return person;
	    }

		[HttpGet("{id}/create/{title}")]
		// GET api/person/ardinheli
		public async Task<Person> Create(string id, string title)
		{
			var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/PersonActorService");

			var serviceProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);
			var serviceProxy = serviceProxyFactory.CreateActorServiceProxy<IPersonActorService>(
				serviceUri,
				new ActorId(id));

			await serviceProxy.CreatePerson(id, title, CancellationToken.None);

			FG.ServiceFabric.Actors.Client.ActorProxyFactory actorProxyFactory = null;
			IPersonActor actorProxy = null;
			actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);

			actorProxy = actorProxyFactory.CreateActorProxy<IPersonActor>(
				serviceUri,
				new ActorId(id));

			var person = await actorProxy.GetPersonAsync(CancellationToken.None);

			return person;
		}		
	}
}
