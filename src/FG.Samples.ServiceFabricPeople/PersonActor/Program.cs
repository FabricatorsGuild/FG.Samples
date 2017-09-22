using System;
using System.Fabric;
using System.Threading;
using Application;
using FG.ServiceFabric.Actors.Runtime;
using FG.ServiceFabric.DocumentDb.CosmosDb;
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Services.Runtime.StateSession;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace PersonActor
{
	internal static class Program
	{
		/// <summary>
		/// This is the entry point of the service host process.
		/// </summary>
		private static void Main()
		{
			try
			{
				// This line registers an Actor Service to host your actor class with the Service Fabric runtime.
				// The contents of your ServiceManifest.xml and ApplicationManifest.xml files
				// are automatically populated when you build this project.
				// For more information, see https://aka.ms/servicefabricactorsplatform

				ActorRuntime.RegisterActorAsync<PersonActor>(
					(context, actorType) => {
						ApplicationInsightsSetup.Setup(ApplicationInsightsSettingsProvider.FromServiceFabricContext(context));
						return new PersonActorService(context, actorType, stateProvider:
							new StateSessionActorStateProvider(context, CreateStateManager(context), actorType),
							settings:
							new ActorServiceSettings()
							{
								ActorGarbageCollectionSettings =
									new ActorGarbageCollectionSettings(10, 2)
							});
					}).GetAwaiter().GetResult();

				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception e)
			{
				ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
				throw;
			}
		}

		private static IStateSessionManager CreateStateManager(StatefulServiceContext context)
		{
			return new DocumentDbStateSessionManager(
					StateSessionHelper.GetServiceName(context.ServiceName),
					context.PartitionId,
					StateSessionHelper.GetPartitionInfo(context,
						() => new FabricClientQueryManagerPartitionEnumerationManager(new FabricClient())).GetAwaiter().GetResult(),
					new CosmosDbSettingsProvider(context)
				);

			//return new FileSystemStateSessionManager(
			//	StateSessionHelper.GetServiceName(context.ServiceName),
			//	context.PartitionId,
			//	StateSessionHelper.GetPartitionInfo(context,
			//		() => new FabricClientQueryManagerPartitionEnumerationManager((new FabricClient()).QueryManager)).GetAwaiter().GetResult(),
			//	@"c:/temp/planetsandpeople");

			//return new ReliableStateSessionManager(this.StateManager);
		}
	}
}
