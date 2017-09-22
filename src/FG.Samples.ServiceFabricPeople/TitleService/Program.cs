using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Application;
using FG.ServiceFabric.Actors.Runtime;
using FG.ServiceFabric.DocumentDb.CosmosDb;
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Services.Runtime.StateSession;
using FG.ServiceFabric.Utils;
using Microsoft.ServiceFabric.Services.Runtime;

namespace TitleService
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
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                ServiceRuntime.RegisterServiceAsync("TitleServiceType",
	                context =>
	                {
						ApplicationInsightsSetup.Setup(ApplicationInsightsSettingsProvider.FromServiceFabricContext(context));
						return new TitleService(context, CreateStateManager(context));
	                }).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(TitleService).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
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
