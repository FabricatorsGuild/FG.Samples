using System;
using System.Threading;
using System.Threading.Tasks;
using FG.ServiceFabric.Actors.Client;
using FG.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Client;
using PersonActor.Diagnostics;
using PersonActor.Interfaces;
using TitleService;

namespace PersonActor
{
	[StatePersistence(StatePersistence.Persisted)]
	internal class PersonActor : Actor, IPersonActor
	{
		private readonly Func<IActorDomainLogger> _actorDomainLoggerFactory;
		private readonly Func<ICommunicationLogger> _communicationLoggerFactory;
		private readonly Func<IActorLogger> _actorLoggerFactory;

		private const string DomainStateName = @"state";

		public PersonActor(ActorService actorService, ActorId actorId)
			: base(actorService, actorId)
		{
			_actorDomainLoggerFactory = () => new ActorDomainLogger(this, ServiceRequestContext.Current);
			_communicationLoggerFactory = () => new CommunicationLogger(this.ActorService);
			_actorLoggerFactory = () => new ActorLogger(this);
		}

		protected override async Task OnActivateAsync()
		{
			var actorLoggerFactory = _actorLoggerFactory();

			var firstActivation = false;
			Person person = null;
			using (actorLoggerFactory.ReadState(DomainStateName))
			{
				var state = await this.StateManager.TryGetStateAsync<Person>(DomainStateName, CancellationToken.None);
				if (state.HasValue)
				{
					person = state.Value;
					firstActivation = true;
				}
			}
			actorLoggerFactory.StartActorActive(firstActivation);

			if (person == null)
			{
				using (actorLoggerFactory.WriteState(DomainStateName))
				{
					person = new Person()
					{
						Name = this.GetActorId().GetStringId(),
						Title = "Unknown"
					};
					await this.StateManager.AddStateAsync(
						DomainStateName,
						person,
						CancellationToken.None);
				}
			}
		}

		protected override Task OnDeactivateAsync()
		{
			_actorLoggerFactory().StopActorActive();
			return base.OnDeactivateAsync();
		}

		public async Task<Person> GetPersonAsync(CancellationToken cancellationToken)
		{
			var state = await this.StateManager.TryGetStateAsync<Person>(DomainStateName, cancellationToken);
			return state.Value;
		}

		public async Task SetTitleAsync(string title, CancellationToken cancellationToken)
		{
			_actorDomainLoggerFactory().TitleSet(title);

			var person = await this.StateManager.GetStateAsync<Person>(DomainStateName, cancellationToken);

			if ((person.Title ?? "").Equals(title ?? "")) return;


			var serviceProxyFactory = new FG.ServiceFabric.Services.Remoting.Runtime.Client.ServiceProxyFactory(_communicationLoggerFactory());

			if (person.Title != null)
			{
				var serviceProxy = serviceProxyFactory.CreateServiceProxy<ITitleService>(
					new Uri($"{this.ActorService.Context.CodePackageActivationContext.ApplicationName}/TitleService"),
					new ServicePartitionKey(TitleServicePartitionSelector.GetPartition(person.Title)));

				await serviceProxy.RemoveTitleAsync(this.GetActorId().GetStringId(), person.Title, cancellationToken);

			}

			if (title != null)
			{
				var serviceProxy = serviceProxyFactory.CreateServiceProxy<ITitleService>(
				new Uri($"{this.ActorService.Context.CodePackageActivationContext.ApplicationName}/TitleService"),
				new ServicePartitionKey(TitleServicePartitionSelector.GetPartition(title)));

				await serviceProxy.SetTitleAsync(this.GetActorId().GetStringId(), title, cancellationToken);
			}

			person.Title = title;
			await this.StateManager.SetStateAsync(DomainStateName, person, cancellationToken);
		}
	}
}
