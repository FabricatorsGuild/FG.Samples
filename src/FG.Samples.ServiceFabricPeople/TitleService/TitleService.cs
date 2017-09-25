using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using FG.ServiceFabric.Actors.Runtime;
using FG.ServiceFabric.Services.Remoting.Runtime;
using FG.ServiceFabric.Services.Runtime.State;
using FG.ServiceFabric.Services.Runtime.StateSession;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TitleService.Diagnostics;

namespace TitleService
{

	internal sealed class TitleService : StatefulService, ITitleService, IStatefulServiceMaintenance
	{
        private readonly ICommunicationLogger _communicationLogger;

		private readonly IStatefulServiceStateManager _stateManager;

		private readonly IDictionary<string, PersonStatistics> _personStatistics = new ConcurrentDictionary<string, PersonStatistics>();

		private readonly IStateSessionManager _stateSessionManager;

        public TitleService(StatefulServiceContext context, IStateSessionManager stateSessionManager)
            : base(context)
        {
	        _stateSessionManager = stateSessionManager;
	        _communicationLogger = new CommunicationLogger(this.Context);
			//_stateManager = new DocumentStorageStatefulServiceStateManager(context, new ReliableStateStatefulServiceStateManager(this.StateManager), storageSessionFactory);

        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[]
            {
                this.CreateServiceReplicaListener(this.Context, _communicationLogger),
            };
        }
        
        Task<string[]> ITitleService.GetTitlesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ObjectMother.Titles);
        }

		private string GetStorageKey(string title)
		{
			return $"state_title_{title}";
		}
		

		async Task<string[]> ITitleService.GetPersonsWithTitleAsync(string title, CancellationToken cancellationToken)
        {
			await _stateSessionManager.OpenDictionary<PersonStatistics>(@"titles", cancellationToken);

			PersonStatistics personStatistic = null;
			using (var session = _stateSessionManager.CreateSession())
			{
				var storageKey = GetStorageKey(title);
				var personStatisticValue = await session.TryGetValueAsync<PersonStatistics>(@"titles", storageKey, cancellationToken);
				personStatistic = personStatisticValue.HasValue ? personStatisticValue.Value :
				   new PersonStatistics() { Title = title, Persons = new string[0] };
			}
			return personStatistic?.Persons ?? new string[0];
		}

		async Task ITitleService.SetTitleAsync(string person, string title, CancellationToken cancellationToken)
	    {
			await _stateSessionManager.OpenDictionary<PersonStatistics>(@"titles", cancellationToken);

			using (var session = _stateSessionManager.CreateSession())
			{
				var storageKey = GetStorageKey(title);

				var personStatisticValue = await session.TryGetValueAsync<PersonStatistics>(@"titles", storageKey, cancellationToken);
				var personStatistic = personStatisticValue.HasValue ? personStatisticValue.Value :
					new PersonStatistics() { Title = title, Persons = new string[0] };

				var persons = new List<string>(personStatistic.Persons) {person};
				personStatistic.Persons = persons.ToArray();

				await session.SetValueAsync(@"titles", storageKey, personStatistic, null, cancellationToken);
			}
		}

	    async Task ITitleService.RemoveTitleAsync(string person, string title, CancellationToken cancellationToken)
	    {
			await _stateSessionManager.OpenDictionary<PersonStatistics>(@"titles", cancellationToken);

			using (var session = _stateSessionManager.CreateSession())
			{
				var storageKey = GetStorageKey(title);
				var personStatisticValue = await session.TryGetValueAsync<PersonStatistics>(@"titles", storageKey, cancellationToken);
				 var personStatistic = personStatisticValue.HasValue ? personStatisticValue.Value : 
					 new PersonStatistics() { Title = title, Persons = new string[0] };

				var persons = new List<string>(personStatistic.Persons) { };
				persons.Remove(person);
				personStatistic.Persons = persons.ToArray();

				await session.SetValueAsync(@"titles", storageKey, personStatistic, null, cancellationToken);
			}
		}

		Task<string[]> IStatefulServiceMaintenance.GetStatesAsync()
		{
			return Task.FromResult(new string[]{""});
		}
	}

	public class PersonStatistics
	{
		public string Title { get; set; }
		public string[] Persons { get; set; }
	}

    public static class ObjectMother
    {
        public static string[] Titles = new string[]
        {
            "Doctor",
            "Overlord",
            "Mister",
            "Fraulein",
        };
    }
}
