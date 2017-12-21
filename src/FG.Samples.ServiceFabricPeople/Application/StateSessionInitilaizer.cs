using System.Fabric;
using FG.ServiceFabric.DocumentDb.CosmosDb;
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Services.Runtime.StateSession;
using FG.ServiceFabric.Services.Runtime.StateSession.CosmosDb;
using FG.ServiceFabric.Services.Runtime.StateSession.FileSystem;

namespace Application
{
	public static class StateSessionInitilaizer
	{
		public static IStateSessionManager CreateStateManager(StatefulServiceContext context)
		{
			return new DocumentDbStateSessionManagerWithTransactions(
					StateSessionHelper.GetServiceName(context.ServiceName),
					context.PartitionId,
					StateSessionHelper.GetPartitionInfoUncached(context,
						() => new FabricClientQueryManagerPartitionEnumerationManager(new FabricClient())).GetAwaiter().GetResult(),
					new CosmosDbSettingsProvider(context)
				);

			//return new FileSystemStateSessionManager(
			//	StateSessionHelper.GetServiceName(context.ServiceName),
			//	context.PartitionId,
			//	StateSessionHelper.GetPartitionInfo(context,
			//		() => new FabricClientQueryManagerPartitionEnumerationManager((new FabricClient()))).GetAwaiter().GetResult(),
			//	@"c:/temp/sfp-local1");

			//return new ReliableStateSessionManager(this.StateManager);
		}
	}
}