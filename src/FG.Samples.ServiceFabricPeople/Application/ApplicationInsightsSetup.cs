using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace Application
{
	public static class ApplicationInsightsSetup
	{
		private const string ParsePerformanceCounterPathRegExPattern = @"\\(?<category>[\w ]*)(?>\((?<instance>[^)]*)\)){0,1}\\(?<counterName>[^""]*)";
		private static readonly Regex ParsePerformanceCounterPathRegEx = new Regex(ParsePerformanceCounterPathRegExPattern, RegexOptions.Compiled);
		public static void Setup(ServiceContext context, ApplicationInsightsSettingsProvider settingsProvider)
		{
			 var configuration = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active;
			configuration.InstrumentationKey = settingsProvider.InstrumentationKey;

			var counters = new[]
			{
				new {Counter = "\\Service Fabric Actor Method(*)\\Invocations/Sec", Name = "Actor method invocations/Sec"},
				new {Counter = "\\Service Fabric Actor Method(*)\\Average milliseconds per invocation", Name = "Actor method average milliseconds per invocation"},
				new {Counter = "\\Service Fabric Actor Method(*)\\Exceptions thrown/Sec", Name = "Actor method exceptions thrown/Sec"},
				new {Counter = "\\Service Fabric Actor(*)\\# of actor calls waiting for actor lock", Name = "# of actor calls waiting for actor lock"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds per lock wait", Name = "Actor average milliseconds per lock wait"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds actor lock held", Name = "Actor average milliseconds actor lock held"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds per save state operation", Name = "Actor average milliseconds per save state operation"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds per load state operation", Name = "Actor average milliseconds per load state operation"},
				new {Counter = "\\Service Fabric Actor(*)\\# of outstanding requests", Name = "Actor # of outstanding requests"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds per request", Name = "Actor average milliseconds per request"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds for request deserialization", Name = "Actor average milliseconds for request deserialization"},
				new {Counter = "\\Service Fabric Actor(*)\\Average milliseconds for response serialization", Name = "Actor average milliseconds for response serialization"},
				new {Counter = "\\PhysicalDisk(*)\\Avg. Disk Read Queue Length", Name = "PhysicalDisc Avg. Disk Read Queue Length"},
				new {Counter = "\\PhysicalDisk(*)\\Avg. Disk Write Queue Length", Name = "PhysicalDisc Avg. Disk Write Queue Length"},
				new {Counter = "\\PhysicalDisk(*)\\Avg. Disk sec/Read", Name = "PhysicalDisc Avg. Disk sec/Read"},
				new {Counter = "\\PhysicalDisk(*)\\Avg. Disk sec/Write", Name = "PhysicalDisc Avg. Disk sec/Write"},
				new {Counter = "\\PhysicalDisk(*)\\Disk Reads/sec", Name = "PhysicalDisc Disk Reads/sec"},
				new {Counter = "\\PhysicalDisk(*)\\Disk Read Bytes/sec", Name = "PhysicalDisc Disk Read Bytes/sec"},
				new {Counter = "\\PhysicalDisk(*)\\Disk Writes/sec", Name = "PhysicalDisc Disk Writes/sec"},
				new {Counter = "\\PhysicalDisk(*)\\Disk Write Bytes/sec", Name = "PhysicalDisc Disk Write Bytes/sec"},
				new {Counter = "\\Memory\\Available MBytes", Name = "Memory Available MBytes"},
				new {Counter = "\\PagingFile\\% Usage", Name = "PagingFile % Usage"},
				new {Counter = "\\Processor(_Total)\\% Processor Time", Name = "% Processor Time"},
				new {Counter = "\\Process (per service)\\% Processor Time", Name = "% Processor Time"},
				new {Counter = "\\Process (per service)\\ID Process", Name = "ID Process"},
				new {Counter = "\\Process (per service)\\Private Bytes", Name = "Private Bytes"},
				new {Counter = "\\Process (per service)\\Thread Count", Name = "Thread Count"},
				new {Counter = "\\Process (per service)\\Virtual Bytes", Name = "Virtual Bytes"},
				new {Counter = "\\Process (per service)\\Working Set", Name = "Working Set"},
				new {Counter = "\\Process (per service)\\Working Set - Private", Name = "Working Set - Private"},
				new {Counter = "\\CE_labs_ai_performancecounters(fgmacbook)\\# times Home was loaded", Name = "Home was loaded"}
			};


			var performanceCollectorModule = new Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.PerformanceCollectorModule();

			var performanceCounterCategories = PerformanceCounterCategory.GetCategories();

			foreach (var counter in counters)
			{
				var match = ParsePerformanceCounterPathRegEx.Match(counter.Counter);
				var category = match.Groups["category"]?.Value ?? "";
				var counterName = match.Groups["counterName"]?.Value ?? "";
				var instance = match.Groups["instance"]?.Value ?? "";

				var performanceCounterCategory = performanceCounterCategories.FirstOrDefault(cat => cat.CategoryName.Equals(category, StringComparison.InvariantCultureIgnoreCase));
				if (performanceCounterCategory != null)
				{
					var performanceCounters = new List<PerformanceCounter>();
					if (performanceCounterCategory.CategoryType == PerformanceCounterCategoryType.MultiInstance)
					{
						var instanceNames = performanceCounterCategory.GetInstanceNames();
						foreach (var instanceName in instanceNames)
						{
							performanceCounters.AddRange(performanceCounterCategory.GetCounters(instanceName));
						}
					}
					else
					{
						performanceCounters.AddRange(performanceCounterCategory.GetCounters());
					}

					if (instance == "*")
					{
						var performanceCountersMatching = performanceCounters.Where(cntr =>
							cntr.CounterName.Equals(counterName, StringComparison.InvariantCultureIgnoreCase));

						foreach (var performanceCounter in performanceCountersMatching)
						{
							var instanceName = performanceCounter.InstanceName;
							var name = $"{counter.Name}-{instanceName}";
							var performanceCounterPath = $"\\{performanceCounter.CategoryName}({instanceName})\\{performanceCounter.CounterName}";
							performanceCollectorModule.Counters.Add(
								new Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.PerformanceCounterCollectionRequest(performanceCounterPath, name));
						}
					}
					else if (instance == "per service")
					{
						var serviceInstancePath = $"{context.PartitionId}_{context.ReplicaOrInstanceId}";

						var performanceCountersMatching = performanceCounters.Where(cntr =>
							cntr.CounterName.Equals(counterName, StringComparison.InvariantCultureIgnoreCase) &&
							cntr.InstanceName.StartsWith(serviceInstancePath, StringComparison.InvariantCultureIgnoreCase));

						foreach (var performanceCounter in performanceCountersMatching)
						{
							var instanceName = performanceCounter.InstanceName;
							var name = $"{counter.Name}-{instanceName}";
							var performanceCounterPath = $"\\{performanceCounter.CategoryName}({instanceName})\\{performanceCounter.CounterName}";
							performanceCollectorModule.Counters.Add(
								new Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.PerformanceCounterCollectionRequest(performanceCounterPath, name));
						}
					}
					else
					{
						var performanceCounter = performanceCounters.FirstOrDefault(cntr =>
							cntr.CounterName.Equals(counterName, StringComparison.InvariantCultureIgnoreCase) &&
							cntr.InstanceName.Equals(instance, StringComparison.InvariantCultureIgnoreCase));
						if (performanceCounter != null)
						{
							var instanceName = performanceCounter.InstanceName;
							var name = $"{counter.Name}-{instanceName}";
							var performanceCounterPath = string.IsNullOrWhiteSpace(instanceName)
								? $"\\{performanceCounter.CategoryName}\\{performanceCounter.CounterName}"
								: $"\\{performanceCounter.CategoryName}({instanceName})\\{performanceCounter.CounterName}";
							performanceCollectorModule.Counters.Add(
								new Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.PerformanceCounterCollectionRequest(performanceCounterPath, name));
						}
					}
				}
			}



			foreach (var counter in performanceCollectorModule.Counters)
			{
				Debug.WriteLine($"Collecting {counter.PerformanceCounter}");
			}

			performanceCollectorModule.EnableIISExpressPerformanceCounters = true;
			performanceCollectorModule.Initialize(configuration);


		}
	}

}