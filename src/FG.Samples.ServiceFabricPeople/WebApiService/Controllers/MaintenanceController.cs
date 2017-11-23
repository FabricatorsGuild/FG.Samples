using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FG.ServiceFabric.Fabric;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using PersonActor.Interfaces;

namespace WebApiService.Controllers
{
	public class MaintenanceController : ControllerBase, ILoggableController
	{
		private readonly StatelessServiceContext _context;
		private readonly object _lock = new object();

		private static PartitionHelper _partitionHelper;

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
					_partitionHelper = new PartitionHelper(() => new FabricClientQueryManagerPartitionEnumerationManager(new FabricClient()));
				}
				return _partitionHelper;
			}
		}

		public MaintenanceController(StatelessServiceContext context) : base(context)
		{
			_context = context;
		}

		

		// GET api/values 
		public async Task<IDictionary<string, Uri[]>> Get()
		{
			var fabricClient = new FabricClient();

			var applicationName = _context.CodePackageActivationContext.ApplicationName;
			var serviceList = await fabricClient.QueryManager.GetServiceListAsync(new Uri(applicationName));

			var servicesByKind = serviceList.GroupBy(s => s.ServiceKind.ToString());

			var result = new Dictionary<string, Uri[]>();
			foreach (var servicesOfKind in servicesByKind)
			{
				var services = servicesOfKind.Select(s => s.ServiceName).ToArray();
				result[servicesOfKind.Key] = services;
			}

			return result;
		}

		public async Task<IDictionary<string, ActorId[]>> Get(string id)
		{
			var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{id}");
			var allPersons = new Dictionary<string, ActorId[]>();

			var partitionKeys = await GetOrCreatePartitionHelper().GetInt64Partitions(serviceUri, ServicesCommunicationLogger);
			foreach (var partitionKey in partitionKeys)
			{
				var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);
				var proxy = actorProxyFactory.CreateActorServiceProxy<IActorServiceMaintenance>(
					serviceUri,
					partitionKey.LowKey);

				var persons = await proxy.GetActors(CancellationToken.None);				
				allPersons.Add($"Partition Int64 {partitionKey.LowKey} - {partitionKey.HighKey}", persons);
			}

			return allPersons;
		}

		[Route("api/maintenance/{id}/actor/{actorId}/state")]
		public async Task<State[]> GetActorState(string id, string actorId)
		{
			var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{id}");
			
			var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(ServicesCommunicationLogger);
			var proxy = actorProxyFactory.CreateActorServiceProxy<IActorServiceMaintenance>(
				serviceUri, 
				new ActorId(actorId));

			var states = await proxy.GetStates(new ActorId(actorId), CancellationToken.None);
			
			return states;
		}


	}
}