using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Application;
using FG.ServiceFabric.Services.Remoting.Runtime.Client;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Client;
using PersonActor.Interfaces;
using TitleService;
using WebApiService.Diagnostics;

namespace WebApiService.Controllers
{
	[ServiceRequestActionFilter]
	public class MaintenanceController : ApiController, ILoggableController
	{
		private readonly object _lock = new object();

		private readonly IWebApiLogger _logger;
		private readonly ICommunicationLogger _servicesCommunicationLogger;

		private static PartitionHelper _partitionHelper;

		private readonly ServiceRequestContextWrapperServiceFabricPeople _contextScope;

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

		public IWebApiLogger Logger => _logger;
		public IDisposable RequestLoggingContext { get; set; }

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_contextScope.Dispose();
		}

		public MaintenanceController(StatelessServiceContext context)
		{
			_contextScope = new ServiceRequestContextWrapperServiceFabricPeople(correlationId: Guid.NewGuid().ToString(), userId: "mainframe64/Kapten_rödskägg");

			_logger = new WebApiLogger(context);
			_servicesCommunicationLogger = new CommunicationLogger(context);

			_logger.ActivatingController(_contextScope.CorrelationId, _contextScope.UserId);
		}

		// GET api/values 
		public async Task<IDictionary<string, Uri[]>> Get()
		{
			var fabricClient = new FabricClient();

			var applicationName = FabricRuntime.GetActivationContext().ApplicationName;
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

			var partitionKeys = await GetOrCreatePartitionHelper().GetInt64Partitions(serviceUri, _servicesCommunicationLogger);
			foreach (var partitionKey in partitionKeys)
			{
				var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(_servicesCommunicationLogger);
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
			
			var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(_servicesCommunicationLogger);
			var proxy = actorProxyFactory.CreateActorServiceProxy<IActorServiceMaintenance>(
				serviceUri, 
				new ActorId(actorId));

			var states = await proxy.GetStates(new ActorId(actorId), CancellationToken.None);
			
			return states;
		}


	}
}