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
using Microsoft.Diagnostics.EventFlow.ServiceFabric;
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
			    using (ManualResetEvent terminationEvent = new ManualResetEvent(initialState: false))
			    using (var diagnosticsPipeline = ServiceFabricDiagnosticPipelineFactory.CreatePipeline("FG-Samples-ServiceFabricPeople-TitleService"))
			    {
			        Console.CancelKeyPress += (sender, eventArgs) => Shutdown(diagnosticsPipeline, terminationEvent);

			        AppDomain.CurrentDomain.UnhandledException += (sender, unhandledExceptionArgs) =>
			        {
			            ServiceEventSource.Current.UnhandledException(unhandledExceptionArgs.ExceptionObject?.ToString() ?? "(no exception information)");
			            Shutdown(diagnosticsPipeline, terminationEvent);
			        };

			        ServiceRuntime.RegisterServiceAsync("TitleServiceType",
			            context =>
			            {
			                var service = new TitleService(context, StateSessionInitilaizer.CreateStateManager(context));
			                ApplicationInsightsSetup.Setup(context, ApplicationInsightsSettingsProvider.FromServiceFabricContext(context));
			                return service;
			            }).GetAwaiter().GetResult();

			        ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(TitleService).Name);

                    terminationEvent.WaitOne();
			    }
			}
			catch (Exception e)
			{
				ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
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
