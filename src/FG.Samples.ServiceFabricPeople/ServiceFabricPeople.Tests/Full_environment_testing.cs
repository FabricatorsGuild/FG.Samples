using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FG.Common.Utils;
using FG.ServiceFabric.Actors.Runtime;
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Services.Runtime.StateSession;
using FG.ServiceFabric.Services.Runtime.StateSession.InMemory;
using FG.ServiceFabric.Testing.Mocks;
using FG.ServiceFabric.Testing.Setup;
using FluentAssertions;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Client;
using NUnit.Framework;
using PersonActor.Interfaces;
using TitleService;

namespace ServiceFabricPeople.Tests
{
	// ReSharper disable InconsistentNaming
	public class Full_environment_testing : MockFabricRuntimeIntegratedSetupBase
	{
		private MockFabricRuntime _mockFabricRuntime;
		private IDictionary<string, string> _state = new ConcurrentDictionary<string, string>();

		[SetUp]
		public void Setup()
		{
			_mockFabricRuntime = new MockFabricRuntime() { DisableMethodCallOutput = true };

			var currentPath = System.IO.Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase);
			var applicationProjectPath =    PathExtensions.GetAbsolutePath(currentPath, @"..\..\..\..\FG.Samples.ServiceFabricPeople\FG.Samples.ServiceFabricPeople.sfproj");
			//var applicationManifestPath =   PathExtensions.GetAbsolutePath(currentPath, @"..\..\..\FG.Samples.ServiceFabricPeople\ApplicationPackageRoot\ApplicationManifest.xml");
			//var applicationParametersPath = PathExtensions.GetAbsolutePath(currentPath, @"..\..\..\IC.HeartBeat.Application\TenantApplicationParameters\Cloud_Hjerta.xml");

			base.Setup(_mockFabricRuntime, applicationProjectPath);
		}
		
		[Test]
		public async Task TestStuff()
		{
			var mockFabricApplication = _mockFabricRuntime.GetApplication("FG.Samples.ServiceFabricPeople");

			var personActor = _mockFabricRuntime.ActorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Bono"),
				mockFabricApplication.ApplicationInstanceName);

			await personActor.SetTitleAsync("Singer", CancellationToken.None);

			var person = await personActor.GetPersonAsync(CancellationToken.None);

			person.Name.Should().Be("Bono");
			person.Title.Should().Be("Singer");


			var titleService = _mockFabricRuntime.ServiceProxyFactory.CreateServiceProxy<ITitleService>(
				mockFabricApplication.ApplicationUriBuilder.Build("TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition("Singer")));

			var persons = await titleService.GetPersonsWithTitleAsync("Singer", CancellationToken.None);

			persons.Should().BeEquivalentTo(new []{ "Bono"});
		}


		[Test]
		public async Task TestStuff2()
		{
			var mockFabricApplication = _mockFabricRuntime.GetApplication("FG.Samples.ServiceFabricPeople");

			var personActor = _mockFabricRuntime.ActorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Bono"),
				mockFabricApplication.ApplicationInstanceName);

			await personActor.SetTitleAsync("Singer", CancellationToken.None);

			var person = await personActor.GetPersonAsync(CancellationToken.None);

			person.Name.Should().Be("Bono");
			person.Title.Should().Be("Singer");

			var titleService = _mockFabricRuntime.ServiceProxyFactory.CreateServiceProxy<ITitleService>(
				mockFabricApplication.ApplicationUriBuilder.Build("TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition("Singer")));

			var persons = await titleService.GetPersonsWithTitleAsync("Singer", CancellationToken.None);

			persons.Should().BeEquivalentTo(new[] { "Bono" });


			personActor = _mockFabricRuntime.ActorProxyFactory.CreateActorProxy<IPersonActor>(new ActorId("Bono"),
				mockFabricApplication.ApplicationInstanceName);

			await personActor.SetTitleAsync("Tax-evader", CancellationToken.None);

			person = await personActor.GetPersonAsync(CancellationToken.None);

			person.Name.Should().Be("Bono");
			person.Title.Should().Be("Tax-evader");

			titleService = _mockFabricRuntime.ServiceProxyFactory.CreateServiceProxy<ITitleService>(
				mockFabricApplication.ApplicationUriBuilder.Build("TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition("Singer")));

			persons = await titleService.GetPersonsWithTitleAsync("Singer", CancellationToken.None);

			persons.Should().BeEquivalentTo(new string[0]);

			titleService = _mockFabricRuntime.ServiceProxyFactory.CreateServiceProxy<ITitleService>(
				mockFabricApplication.ApplicationUriBuilder.Build("TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition("Tax-evader")));

			persons = await titleService.GetPersonsWithTitleAsync("Tax-evader", CancellationToken.None);

			persons.Should().BeEquivalentTo(new[] { "Bono" });
		}

		protected override IStateSessionManager CreateStateSessionManager(ServiceContext context)
		{
			return new InMemoryStateSessionManagerWithTransaction(
				StateSessionHelper.GetServiceName(context.ServiceName),
				context.PartitionId,
				StateSessionHelper.GetPartitionInfo(context, () => _mockFabricRuntime.PartitionEnumerationManager).GetAwaiter().GetResult(),
				_state);
		}

		protected override IActorStateProvider CreateActorStateProvider(StatefulServiceContext context, ActorTypeInformation actorTypeInformation)
		{
			return new StateSessionActorStateProvider(context, CreateStateSessionManager(context), actorTypeInformation);			
		}

		protected override object CreateActorServiceParameter(Type actorServiceType, Type parameterType, object defaultValue)
		{
			if (typeof(Func<IPartitionEnumerationManager>).IsAssignableFrom(parameterType))
			{
				return (Func<IPartitionEnumerationManager>) (() => _mockFabricRuntime.PartitionEnumerationManager);
			}

			return base.CreateActorServiceParameter(actorServiceType, parameterType, defaultValue);
		}
	}

	// ReSharper restore InconsistentNaming
}