﻿using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using FG.ServiceFabric.Actors.Runtime;
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
					(context, actorType) => new PersonActorService(context, actorType, stateProvider:
						new StateSessionActorStateProvider(context,
								new FileSystemStateSessionManager(
									context.ServiceName.AbsoluteUri,
									context.PartitionId,
									StateSessionHelper.GetPartitionInfo(context, () => new FabricClientQueryManagerPartitionEnumerationManager(new FabricClient())).GetAwaiter().GetResult(),
									@"c:/temp/sf"), actorType))).GetAwaiter().GetResult();

				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception e)
			{
				ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
				throw;
			}
		}
	}
}
