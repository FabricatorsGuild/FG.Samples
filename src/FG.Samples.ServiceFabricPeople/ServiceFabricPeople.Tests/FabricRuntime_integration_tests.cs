using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FG.Common.Async;
using FG.Common.Utils;
using FG.ServiceFabric.Testing.Mocks;
using FluentAssertions;
using Microsoft.ServiceFabric.Actors;
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

		[SetUp]
		public void Setup_mock_fabricruntime()
		{
			_fabricRuntime = new MockFabricRuntime("Overlord");

			_fabricRuntime.SetupService("TitleService", (context, replica) => new TitleService.TitleService(context));

			_fabricRuntime.SetupActor<PersonActor.PersonActor, PersonActorService>(
				(service, actorId) => new PersonActor.PersonActor(service, actorId),
				(context, actorTypeInformation, stateProvider, stateManagerFactory) =>
					new PersonActorService(context, actorTypeInformation, stateProvider: stateProvider));
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

			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);
			var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
				ServicePartitionKey.Singleton);
			var titles = await titleService.GetTitlesAsync(ct);
			var title = titles.Single(t => t.Equals("doctor", StringComparison.InvariantCultureIgnoreCase));

			var actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
			var actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Karl-Henrik"));
			await actor.SetTitleAsync(title, ct);

			actorProxyFactory = new FG.ServiceFabric.Actors.Client.ActorProxyFactory(logger);
			actor = actorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Karl-Henrik"));
			var person = await actor.GetPersonAsync(ct);

			person.Title.Should().Be("Doctor");
		}

		[Test]
		public async Task TitleService_should_update_persons_by_title_when_title_is_set_on_person()
		{
			var ct = CancellationToken.None;

			var logger = new CommunicationLogger(Guid.NewGuid().ToString().Substring(0, 6).ToMD5().ToUpperInvariant());

			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);
			var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
				ServicePartitionKey.Singleton);
			var titles = await titleService.GetTitlesAsync(ct);

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
			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(logger);
			var titleService = serviceProxyFactory.CreateServiceProxy<ITitleService>(new Uri("fabric:/Overlord/TitleService"),
				ServicePartitionKey.Singleton);
			var titles = await titleService.GetTitlesAsync(ct);

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
				ServicePartitionKey.Singleton);

			var personsWithTitleAsync = await titleService.GetPersonsWithTitleAsync("Doctor", ct);
			personsWithTitleAsync.ShouldBeEquivalentTo(new[] { "Mikey", "Brand", "Chunk", "Mouth", "Andy", "Stef", "Data", "Navarre" });

			personsWithTitleAsync = await titleService.GetPersonsWithTitleAsync("Fraulein", ct);
			personsWithTitleAsync.ShouldBeEquivalentTo(new[] { "Gaston", "Isabeau", "Imperius", "Bishop", "Marquet" });
		}
	}
	// ReSharper restore InconsistentNaming
}
