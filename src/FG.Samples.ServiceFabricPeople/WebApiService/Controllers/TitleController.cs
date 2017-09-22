using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Application;
using FG.Common.Utils;
using FG.ServiceFabric.Services.Remoting.Runtime.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using TitleService;
using WebApiService.Diagnostics;

namespace WebApiService.Controllers
{
	[Route("api/[controller]")]
	public class TitleController : ControllerBase, ILoggableController
	{
		private readonly object _lock = new object();

		private static PartitionHelper _partitionHelper;

		private PartitionHelper GetOrCreatePartitionHelper()
		{
			if (_partitionHelper != null)
			{
				return _partitionHelper;
			}

			lock (_lock)
			{
				if (_partitionHelper == null)
				{
					_partitionHelper = new PartitionHelper();
				}
				return _partitionHelper;
			}
		}


		public TitleController(StatelessServiceContext context) : base(context)
		{
		}


		[HttpGet]
		// GET api/values 
		public async Task<IDictionary<string, IList<string>>> Get()
		{
			var serviceUri = new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/TitleService");
			var allPersons = new Dictionary<string, IList<string>>();

			var ct = CancellationToken.None;
			var partitionKeys = await GetOrCreatePartitionHelper().GetInt64Partitions(serviceUri, ServicesCommunicationLogger);
			foreach (var partitionKey in partitionKeys)
			{
				var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(ServicesCommunicationLogger);
				var proxy = serviceProxyFactory.CreateServiceProxy<ITitleService>(
					serviceUri, 
					new ServicePartitionKey(partitionKey.LowKey));

				var partition = $"{partitionKey.LowKey}-{partitionKey.HighKey}";
				var partitionTitles = new Dictionary<string, IDictionary<string, string>>();

				var titles = await proxy.GetTitlesAsync(ct);
				foreach (var title in titles)
				{
					var persons = await proxy.GetPersonsWithTitleAsync(title, ct);
					var personsByTitle = new List<string>(persons);

					if (allPersons.ContainsKey(title))
					{
						personsByTitle.AddRange(allPersons[title]);
					}
					allPersons[title] = personsByTitle;
				}
			}

			return allPersons;
		}

	}
}