﻿using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Microsoft.Diagnostics.EventFlow.ServiceFabric;

namespace WebApiService
{
    internal static class Program
    {

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

                    ServiceRuntime.RegisterServiceAsync("WebApiServiceType",
                        context =>
                        {
                            var service = new WebApiService(context, diagnosticsPipeline);
                            ApplicationInsightsSetup.Setup(context, ApplicationInsightsSettingsProvider.FromServiceFabricContext(context));
                            return service;
                        }).GetAwaiter().GetResult();

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebApiService).Name);

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
