using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Application;
using FG.ServiceFabric.Actors.Remoting.Runtime;
using FG.ServiceFabric.Services.Remoting.FabricTransport;
using FG.ServiceFabric.Services.Remoting.Runtime.Client;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using PersonActor.Diagnostics;
using PersonActor.Interfaces;
using TitleService;
using FG.ServiceFabric.Actors.Runtime;

namespace PersonActor
{
	public class PersonActorService : ActorService, IPersonActorService, IActorServiceMaintenance
	{
		private readonly Func<IServiceDomainLogger> _serviceLoggerFactory;
		private readonly Func<ICommunicationLogger> _communicationLoggerFactory;

		public PersonActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null,
			Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null,
			ActorServiceSettings settings = null) : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
		{
			_serviceLoggerFactory = () => new ServiceDomainLogger(this, ServiceRequestContext.Current);
			_communicationLoggerFactory = () => new CommunicationLogger(this);

			_actorStates.Add("state", typeof(Person));
		}

		protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
		{
			return new[]
			{
				this.CreateServiceReplicaListener(_communicationLoggerFactory()),
			};
		}
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
					_partitionHelper = new PartitionHelper();
				}
				return _partitionHelper;
			}
		}

		protected override async Task RunAsync(CancellationToken cancellationToken)
		{
			var communicationLogger = _communicationLoggerFactory();

			var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/TitleService");
			var partitionKeys = await GetOrCreatePartitionHelper().GetInt64Partitions(serviceUri, communicationLogger);
			var allTitlesList = new List<string>();

			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(communicationLogger);
			foreach (var partitionKey in partitionKeys)
			{
				var serviceProxy = serviceProxyFactory.CreateServiceProxy<ITitleService>(
					new Uri($"{this.Context.CodePackageActivationContext.ApplicationName}/TitleService"),
					new ServicePartitionKey(partitionKey.LowKey));
				var titles = await serviceProxy.GetTitlesAsync(cancellationToken);
				allTitlesList.AddRange(titles);
			}
			var allTitles = allTitlesList.ToArray();

			while (true)
			{
				foreach (var name in ObjectMother.Names)
				{
					var correlationId = Guid.NewGuid().ToString();
					using (new ServiceRequestContextWrapperServiceFabricPeople(correlationId, Environment.UserName))
					{
						var serviceLogger = _serviceLoggerFactory();
						communicationLogger = _communicationLoggerFactory();
						using (serviceLogger.RunAsyncLoop())
						{
							try
							{
								var title = allTitles[Environment.TickCount % allTitles.Length];

								var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(communicationLogger);
								var proxy = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(name));
								await proxy.SetTitleAsync(title, cancellationToken);

								serviceLogger.PersonGenerated(name, title);
							}
							catch (Exception ex)
							{
								serviceLogger.RunAsyncLoopFailed(ex);
							}
						}
					}

					await Task.Delay(10000, cancellationToken);
				}
			}
		}

		public async Task<IDictionary<string, Person>> GetPersons(CancellationToken cancellationToken)
		{
			ContinuationToken continuationToken = null;
			//var actors = await this.StateProvider.GetActorsAsync(100, continuationToken, cancellationToken);

			var results = 0;
			var maxResults = 500;
			var result = new Dictionary<string, Person>();
			do
			{
				var page = await this.StateProvider.GetActorsAsync(100, continuationToken, cancellationToken);
				foreach (var actor in page.Items)
				{
					var actorState = await this.StateProvider.LoadStateAsync<Person>(actor, "state", cancellationToken);
					result.Add(actor.GetStringId(), actorState);
					results++;
				}
				if (results >= maxResults) return result;
				continuationToken = page.ContinuationToken;
			} while (continuationToken != null);

			return result;
		}




		private readonly IDictionary<string, Type> _actorStates = new ConcurrentDictionary<string, Type>();

		public async Task<State[]> GetStates(ActorId actorId, CancellationToken cancellationToken)
		{
			var states = new List<State>();
			var stateNamesEnumerator = await this.StateProvider.EnumerateStateNamesAsync(actorId, cancellationToken);

			foreach (var stateName in stateNamesEnumerator)
			{
				var actorStateType = _actorStates[stateName];
				var actorState = (Task) await this.StateProvider.LoadStateAsync(actorStateType, actorId, stateName, cancellationToken);
				var actorStateResult = actorState.GetTaskResult(actorStateType);
				var stateData = Newtonsoft.Json.JsonConvert.SerializeObject(actorStateResult);

				var state = new State() {Data = stateData, Name = stateName, TypeName = actorStateType.FullName};
				states.Add(state);
			}
			return states.ToArray();

		}

		public async Task<ActorId[]> GetActors(CancellationToken cancellationToken)
		{
			ContinuationToken continuationToken = null;
			//var actors = await this.StateProvider.GetActorsAsync(100, continuationToken, cancellationToken);

			var results = 0;
			var maxResults = 50000;
			var result = new List<ActorId>();
			do
			{
				var page = await this.StateProvider.GetActorsAsync(100, continuationToken, cancellationToken);
				foreach (var actor in page.Items)
				{
					result.Add(actor);
					results++;
				}
				if (results >= maxResults) return result.ToArray();
				continuationToken = page.ContinuationToken;
			} while (continuationToken != null);

			return result.ToArray();
		}
	}
}