using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Application;
using FG.ServiceFabric.Actors.Runtime;
using FG.ServiceFabric.DocumentDb.CosmosDb;
using FG.ServiceFabric.Fabric;
using FG.ServiceFabric.Services.Runtime.StateSession;
using FG.ServiceFabric.Utils;
using Microsoft.Diagnostics.EventFlow.ServiceFabric;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TitleService;

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
			    using (ManualResetEvent terminationEvent = new ManualResetEvent(initialState: false))
			    using (var diagnosticsPipeline = ServiceFabricDiagnosticPipelineFactory.CreatePipeline("FG-Samples-ServiceFabricPeople-PersonActor"))
			    {
			        Console.CancelKeyPress += (sender, eventArgs) => Shutdown(diagnosticsPipeline, terminationEvent);

			        AppDomain.CurrentDomain.UnhandledException += (sender, unhandledExceptionArgs) =>
			        {
			            ActorEventSource.Current.Message(unhandledExceptionArgs.ExceptionObject?.ToString() ?? "(no exception information)");
			            Shutdown(diagnosticsPipeline, terminationEvent);
			        };

			        ActorRuntime.RegisterActorAsync<PersonActor>(
			            (context, actorType) => {
			                var service = new PersonActorService(context, actorType, stateProvider:
			                    //ActorStateProviderHelper.CreateDefaultStateProvider(actorType), //
                                new OverloadedStateSessionActorStateProvider(ActorStateProviderHelper.CreateDefaultStateProvider(actorType), StateSessionInitilaizer.CreateStateManager(context)),
			                    settingsProvider: new ServiceConfigSettingsProvider(context),
			                    settings: new ActorServiceSettings()
			                    {
			                        ActorGarbageCollectionSettings =
			                            new ActorGarbageCollectionSettings(10, 2)
			                    });
			                ApplicationInsightsSetup.Setup(context, ApplicationInsightsSettingsProvider.FromServiceFabricContext(context));
			                return service;
			            }).GetAwaiter().GetResult();


                    ActorEventSource.Current.Message("Registered actor service {1} in process {0}", Process.GetCurrentProcess().Id, typeof(PersonActorService).Name);

			        terminationEvent.WaitOne();
			    }                
			}
			catch (Exception e)
			{
				ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
				throw;
			}
        }
        private static void Shutdown(IDisposable disposable, ManualResetEvent terminationEvent)
        {
            try
            {
                disposable.Dispose();
            }
            finally
            {
                terminationEvent.Set();
            }
        }
    }
}
