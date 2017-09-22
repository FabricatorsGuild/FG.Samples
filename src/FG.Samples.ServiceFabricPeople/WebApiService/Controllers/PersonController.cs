using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using FG.ServiceFabric.Services.Remoting.Runtime.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using PersonActor.Interfaces;

namespace WebApiService.Controllers
{
	[Route("api/[controller]")]
	public class PersonController : ControllerBase, ILoggableController
    {
        private readonly object _lock = new object();

        private static PartitionHelper _partitionHelper;

        public PersonController(StatelessServiceContext context) : base(context)
        {
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
                    _partitionHelper = new PartitionHelper();
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
            foreach (var partitionKey in partitionKeys)
            {
                var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);
                var proxy = actorProxyFactory.CreateActorServiceProxy<IPersonActorService>(
                    serviceUri,
                    partitionKey.LowKey);

                var persons = await proxy.GetPersons(CancellationToken.None);
                allPersons.Add(partitionKey.LowKey.ToString(), persons);
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
			else if("settitle".Equals(operation, StringComparison.InvariantCultureIgnoreCase))
			{
				var title = payload;

				actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);

				actorProxy = actorProxyFactory.CreateActorProxy<IPersonActor>(
					serviceUri,
					new ActorId(id));

				await actorProxy.SetTitleAsync(title, CancellationToken.None);
			}

			actorProxyFactory  = actorProxyFactory ?? new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);

			actorProxy = actorProxy ?? actorProxyFactory.CreateActorProxy<IPersonActor>(
				serviceUri,
				new ActorId(id));

			var person = await actorProxy.GetPersonAsync(CancellationToken.None);

			return person;
		}
	}
}
