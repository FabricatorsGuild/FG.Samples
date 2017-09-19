using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application;
using FG.Common.Async;
using FG.Common.Utils;
using FG.ServiceFabric.Actors.Runtime;
using FG.ServiceFabric.Services.Remoting.FabricTransport;
using FG.ServiceFabric.Services.Runtime;
using FG.ServiceFabric.Services.Runtime.State;
using FG.ServiceFabric.Services.Runtime.StateSession;
using FG.ServiceFabric.Testing.Mocks;
using FG.ServiceFabric.Testing.Mocks.Actors.Runtime;
using FG.ServiceFabric.Testing.Mocks.Data;
using FG.ServiceFabric.Testing.Mocks.Fabric;
using FG.ServiceFabric.Testing.Mocks.Services.Runtime;
using FluentAssertions;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Client;
using NUnit.Framework;
using PersonActor;
using PersonActor.Interfaces;
using TitleService;

namespace ServiceFabricPeople.Tests
{
	// ReSharper disable InconsistentNaming
    public class FabricRuntime_integration_tests
	{
		private MockFabricRuntime _fabricRuntime;
		private ServiceRequestContextWrapperServiceFabricPeople _context;

		private static string[][] _names = new []
			{
				new[] {"Mikey", "Brand", "Chunk", "Mouth", "Andy", "Stef", "Data"},
				new[] {"Sardo Numpspa", "Chandler Jarrell", "Kee Nang"},
				new[] {"Madmartigan", "Sorsha", "Queen Bavmorda", "Willow Ufgood", "Megosh"},
				new[] {"Gaston", "Navarre", "Isabeau", "Imperius", "Bishop", "Marquet"}
			};

		private readonly IList<string> _actionsPerformed = new List<string>();
		//private MockActorStateProvider _actorStateProvider;

		[SetUp]
		public void Setup_mock_fabricruntime()
		{
			_fabricRuntime = new MockFabricRuntime("Overlord");

			_context = new ServiceRequestContextWrapperServiceFabricPeople(Guid.NewGuid().ToString(), "testivus");
			var reliableStateManager = new MockReliableStateManager(_fabricRuntime);

			var state = new Dictionary<string, string>();

			_fabricRuntime.SetupService((context, stateManager) => new TitleService.TitleService(context, 
				stateSessionManager: new InMemoryStateSessionManager(
					StateSessionHelper.GetServiceName(context.ServiceName),
					context.PartitionId,
					StateSessionHelper.GetPartitionInfo(context, () => _fabricRuntime.PartitionEnumerationManager).GetAwaiter().GetResult(),
					state)), 
				serviceDefinition: MockServiceDefinition.CreateUniformInt64Partitions(10));

			_fabricRuntime.SetupActor<PersonActor.PersonActor, PersonActorService>(
				(service, actorId) => new PersonActor.PersonActor(service, actorId),
				(context, actorTypeInformation, stateProvider, stateManagerFactory) =>
					new PersonActorService(context, actorTypeInformation,
						stateProvider: stateProvider),
				createActorStateProvider: (context, actorTypeInformation) =>
					new StateSessionActorStateProvider(context,
						stateSessionManager: new InMemoryStateSessionManager(
							StateSessionHelper.GetServiceName(context.ServiceName),
							context.PartitionId,
							StateSessionHelper.GetPartitionInfo(context, () => _fabricRuntime.PartitionEnumerationManager).GetAwaiter().GetResult(),
							state),
						actorTypeInfo: actorTypeInformation),
				serviceDefinition: MockServiceDefinition.CreateUniformInt64Partitions(10, long.MinValue, long.MaxValue));

			Console.WriteLine($"Running with Mock Fabric Runtime {_fabricRuntime.ApplicationName} - {_fabricRuntime.GetHashCode()}");
		}

		[TearDown]
		public void Teardown_mock_fabricruntime()
		{
			_context?.Dispose();
		}

		[Test]
		public async Task Should_be_able_to_get_actor_state_from_Actor()
		{
			var person = await ExecutionHelper.ExecuteWithRetriesAsync((ct) =>
			{
				var actor = _fabricRuntime.ActorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Karl-Henrik"));
				return actor.GetPersonAsync(ct);
			}, 3, TimeSpan.FromMilliseconds(3), CancellationToken.None);

			person.Name.Should().Be("Karl-Henrik");
		}

		[Test]
		public async Task Should_be_able_to_set_title_on_person()
		{
			var ct = CancellationToken.None;
			
			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			var title = "doctor";

			var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
			var actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Karl-Henrik"));
			await actor.SetTitleAsync(title, ct);

			actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
			actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Karl-Henrik"));
			var person = await actor.GetPersonAsync(ct);

			person.Title.Should().Be(title);
		}

		[Test]
		public async Task TitleService_should_update_persons_by_title_when_title_is_set_on_person()
		{
			var ct = CancellationToken.None;

			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);

			var titles = new List<string>();
			var titlePartitions = await _fabricRuntime.PartitionEnumerationManager.GetPartitionListAsync(new Uri("fabric:/Overlord/TitleService"));
			foreach (var titlePartition in titlePartitions)
			{
				var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
					new ServicePartitionKey((titlePartition.PartitionInformation as Int64RangePartitionInformation).LowKey));
				titles.AddRange(await titleService.GetTitlesAsync(ct));
			}

			var names = new[]
			{
				new[] {"Mikey", "Brand", "Chunk", "Mouth", "Andy", "Stef", "Data"},
				new[] {"Sardo Numpspa", "Chandler Jarrell", "Kee Nang"},
				new[] {"Madmartigan", "Sorsha", "Queen Bavmorda", "Willow Ufgood", "Megosh"},
				new[] {"Gaston", "Navarre", "Isabeau", "Imperius", "Bishop", "Marquet"}
			};
			var i = 0;
			foreach (var title in titles)
			{
				foreach (var name in names[i % names.Length])
				{
					var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
					var actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(name));
					await actor.SetTitleAsync(title, ct);
				}
				i++;
			}

			i = 0;
			foreach (var title in titles)
			{
				Console.WriteLine($"{title}");
				var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
					new ServicePartitionKey(TitleServicePartitionSelector.GetPartition(title)));
				var personsWithTitleAsync = await titleService.GetPersonsWithTitleAsync(title, ct);
				foreach (var name in personsWithTitleAsync)
				{
					Console.WriteLine($"\t{name}");
				}

				personsWithTitleAsync.ShouldAllBeEquivalentTo(names[i % names.Length]);
				i++;
			}
		}

		private async Task Setup_persons_with_names_and_titles(ICommunicationLogger logger, CancellationToken ct)
		{
			var serviceUri = new Uri("fabric:/Overlord/TitleService");
			var serviceProxyFactory = _fabricRuntime.ServiceProxyFactory;// new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);

			var partitionKeys = new List<long>();
			foreach (var partition in await _fabricRuntime.PartitionEnumerationManager.GetPartitionListAsync(serviceUri))
			{
				var partitionInfo = partition.PartitionInformation as Int64RangePartitionInformation;
				if (partitionInfo == null)
				{
					throw new InvalidOperationException($"The service {serviceUri} should have a uniform Int64 partition. Instead: {partition.PartitionInformation.Kind}");
				}
				partitionKeys.Add(partitionInfo.LowKey);
			}

			var titles = new List<string>();
			foreach (var partitionKey in partitionKeys)
			{
				var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(serviceUri, new ServicePartitionKey(partitionKey));
				titles.AddRange(await titleService.GetTitlesAsync(ct));
			}

			var i = 0;
			foreach (var title in titles)
			{
				foreach (var name in _names[i % _names.Length])
				{
					var actorProxyFactory = _fabricRuntime.ActorProxyFactory; // new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
					var actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId(name));
					await actor.SetTitleAsync(title, ct);
				}
				i++;
			}

			i = 0;
			foreach (var title in titles)
			{
				var partitionKey = TitleServicePartitionSelector.GetPartition(title);
				Console.WriteLine($"{title} - {partitionKey}");
				var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(serviceUri, new ServicePartitionKey(partitionKey));
				var personsWithTitleAsync = await titleService.GetPersonsWithTitleAsync(title, ct);
				foreach (var name in personsWithTitleAsync)
				{
					Console.WriteLine($"\t{name}");
				}
				i++;
			}
		}

		[Test]
		public async Task TitleService_should_update_persons_by_title_when_title_is_changed_on_person()
		{
			var ct = CancellationToken.None;
			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			await Setup_persons_with_names_and_titles(logger, ct);

			var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
			var actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Navarre"));
			await actor.SetTitleAsync("Doctor", ct);

			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);
			var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition("Doctor")));

			var personsWithTitleAsync = await titleService.GetPersonsWithTitleAsync("Doctor", ct);
			personsWithTitleAsync.ShouldBeEquivalentTo(new[] { "Mikey", "Brand", "Chunk", "Mouth", "Andy", "Stef", "Data", "Navarre" });

			titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition("Fraulein")));

			personsWithTitleAsync = await titleService.GetPersonsWithTitleAsync("Fraulein", ct);
			personsWithTitleAsync.ShouldBeEquivalentTo(new[] { "Gaston", "Isabeau", "Imperius", "Bishop", "Marquet" });
		}


		[Test]
		public async Task PersonService_Maintenance_should_return_all_Actors()
		{
			var ct = CancellationToken.None;

			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			var actorIds = new List<ActorId>();
			var partitionList = await _fabricRuntime.PartitionEnumerationManager.GetPartitionListAsync(new Uri("fabric:/Overlord/PersonActorService"));
			foreach (var partition in partitionList)
			{
				var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);
				var personActorMaintenance = serviceProxyFactory.CreateServiceProxy<IActorServiceMaintenance>(new Uri("fabric:/Overlord/PersonActorService"),
					new ServicePartitionKey((partition.PartitionInformation as Int64RangePartitionInformation).LowKey));
				actorIds.AddRange(await personActorMaintenance.GetActors(ct));
			}

			actorIds.Should().BeEmpty();
		}


		[Test]
		public async Task PersonService_Maintenance_should_return_all_Actors_after_actors_have_heen_created()
		{
			var ct = CancellationToken.None;

			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			await Setup_persons_with_names_and_titles(logger, ct);
			var actorIds = new List<ActorId>();
			var partitionList = await _fabricRuntime.PartitionEnumerationManager.GetPartitionListAsync(new Uri("fabric:/Overlord/PersonActorService"));
			foreach (var partition in partitionList)
			{
				var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);
				var personActorMaintenance = serviceProxyFactory.CreateServiceProxy<IActorServiceMaintenance>(new Uri("fabric:/Overlord/PersonActorService"),
					new ServicePartitionKey((partition.PartitionInformation as Int64RangePartitionInformation).LowKey));
				actorIds.AddRange(await personActorMaintenance.GetActors(ct));
			}

			var allNames = _names.SelectMany(n => n).Select(n => new ActorId(n));

			actorIds.Should().BeEquivalentTo(allNames);

			foreach (var actionPerformed in _actionsPerformed)
			{
				Console.WriteLine(actionPerformed);
			}
		}

		[Test]
		public async Task PersonService_Maintenance_should_return_all_Actor_state()
		{
			var ct = CancellationToken.None;

			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			await Setup_persons_with_names_and_titles(logger, ct);

			var serviceProxyFactory = _fabricRuntime.ServiceProxyFactory;
			var titleService = serviceProxyFactory.CreateServiceProxy<IActorServiceMaintenance>(new Uri("fabric:/Overlord/PersonActorService"),
				new ServicePartitionKey(new ActorId("Bishop").GetPartitionKey()));
			var states = await titleService.GetStates(new ActorId("Bishop"),  ct);

			foreach (var state in states)
			{
				Console.WriteLine($"\"{state.Name}\" [{state.TypeName}]");
				Console.WriteLine(state.Data);
			}

			states.Should().NotBeNull();
		}

	}
	// ReSharper restore InconsistentNaming
}
