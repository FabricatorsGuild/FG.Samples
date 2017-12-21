using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application;
using FG.Common.Utils;
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
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Utils;

namespace PersonActor
{
	public class PersonActorService : Microsoft.ServiceFabric.Actors.Runtime.ActorService, IPersonActorService, IActorServiceMaintenance
	{
		private readonly ISettingsProvider _settingsProvider;
		private readonly Func<IPartitionEnumerationManager> _partitionEnumerationManagerFactory;
		private readonly Func<IServiceDomainLogger> _serviceLoggerFactory;
		private readonly Func<ICommunicationLogger> _communicationLoggerFactory;

		public PersonActorService(
			StatefulServiceContext context, 
			ActorTypeInformation actorTypeInfo,
			ISettingsProvider settingsProvider,
			Func<Microsoft.ServiceFabric.Actors.Runtime.ActorService, ActorId, Microsoft.ServiceFabric.Actors.Runtime.ActorBase> actorFactory = null,
			Func<Microsoft.ServiceFabric.Actors.Runtime.ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, 
			IActorStateProvider stateProvider = null,
			ActorServiceSettings settings = null,
			Func<IPartitionEnumerationManager> partitionEnumerationManagerFactory = null
			) : 
			base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
		{
			_settingsProvider = settingsProvider;
			_partitionEnumerationManagerFactory = partitionEnumerationManagerFactory ?? (() => new FabricClientQueryManagerPartitionEnumerationManager(new FabricClient()));
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

		private string[] _allTitles;

		internal ServiceProxyFactory GetServiceProxyFactory(ICommunicationLogger communicationLogger)
		{	
			return new ServiceProxyFactory(communicationLogger);
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

		protected override async Task RunAsync(CancellationToken cancellationToken)
		{
		    await base.RunAsync(cancellationToken);

			var communicationLogger = _communicationLoggerFactory();

			var serviceUri = new Uri($"{this.Context.CodePackageActivationContext.ApplicationName}/TitleService");
			var partitionKeys = await GetOrCreatePartitionHelper().GetInt64Partitions(serviceUri, communicationLogger);
			var allTitlesList = new List<string>();

			var serviceProxyFactory = new ServiceProxyFactory(communicationLogger);
			foreach (var partitionKey in partitionKeys)
			{
				var correlationId = Guid.NewGuid().ToString();
				var userName = Environment.UserName;
				var authToken = $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}|{userName}|{correlationId}".ToBase64();
				var tenantId = "04D5C3F2-F26C-4EF1-A1FE-FC70BBA427FA";
				using (new ServiceFabricPeopleContext(correlationId, userName, authToken, tenantId))
				{
					var serviceProxy = serviceProxyFactory.CreateServiceProxy<ITitleService>(
						new Uri($"{this.Context.CodePackageActivationContext.ApplicationName}/TitleService"),
						new ServicePartitionKey(partitionKey.LowKey));
					var titles = await serviceProxy.GetTitlesAsync(cancellationToken);
					allTitlesList.AddRange(titles);
				}
			}
			_allTitles = allTitlesList.ToArray();

			var random = new Random(Environment.TickCount);
			var names = ObjectMother.Names.Select(n => new {Name = n, Ordinal = random.Next(1000)}).OrderBy(i => i.Ordinal).Select(i => i.Name).ToArray();
			var firstNames = ObjectMother.FirstNames.Select(n => new { Name = n, Ordinal = random.Next(1000) }).OrderBy(i => i.Ordinal).Select(i => i.Name).ToArray();
			var namesCount = names.Length;
			var firstNamesCount = firstNames.Length;
			var iName = 0;
			while (true)
			{
				await Task.Delay(1000000, cancellationToken);
				var name = $"{firstNames[iName % firstNamesCount]} {names[iName % namesCount]}";
				iName++;
				var correlationId = Guid.NewGuid().ToString();
				var userName = Environment.UserName;
				var authToken = $"{correlationId}|{userName}|{DateTime.UtcNow}".ToBase64();
				var tenantId = "04D5C3F2-F26C-4EF1-A1FE-FC70BBA427FA";
				using (new ServiceFabricPeopleContext(correlationId, userName, authToken, tenantId))
				{
					var serviceLogger = _serviceLoggerFactory();
					communicationLogger = _communicationLoggerFactory();
					using (serviceLogger.RunAsyncLoop())
					{
						try
						{
							var title = _allTitles[Environment.TickCount % _allTitles.Length];

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
			}
		}

		public async Task<string> CreateRandomPerson(CancellationToken cancellationToken)
		{
			var iName = Environment.TickCount;
			var namesCount = ObjectMother.Names.Length;
			var firstNamesCount = ObjectMother.FirstNames.Length;
			var name = $"{ObjectMother.FirstNames[iName % firstNamesCount]} {ObjectMother.Names[iName % namesCount]}";
			var title = _allTitles[Environment.TickCount % _allTitles.Length];

			var serviceLogger = _serviceLoggerFactory();
			var communicationLogger = _communicationLoggerFactory();
			try
			{
				var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(communicationLogger);
				var proxy = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(name));
				await proxy.SetTitleAsync(title, cancellationToken);

				serviceLogger.PersonGenerated(name, title);

				return $"{title} {name}";
			}
			catch (Exception ex)
			{
				throw new Exception($"CreatePerson {name} failed", ex);
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

		public async Task<string> CreatePerson(string name, string title, CancellationToken cancellationToken)
		{
			var serviceLogger = _serviceLoggerFactory();
			var communicationLogger = _communicationLoggerFactory();
			try
			{
				var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(communicationLogger);
				var proxy = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(name));
				await proxy.SetTitleAsync(title, cancellationToken);

				serviceLogger.PersonGenerated(name, title);

				return $"{title} {name}";
			}
			catch (Exception ex)
			{
				throw new Exception($"CreatePerson {name} failed", ex);
			}
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