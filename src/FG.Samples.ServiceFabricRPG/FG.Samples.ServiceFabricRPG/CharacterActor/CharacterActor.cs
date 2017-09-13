using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using CharacterActor.Interfaces;
using FG.Common.Async;
using FG.Common.Utils;
using FG.CQRS;
using FG.ServiceFabric.Actors.Runtime;

namespace CharacterActor
{

	#region Domain event interfaces

	public interface IRootEvent : IAggregateRootEvent
	{
	}

	public interface ICreatedEvent : IRootEvent
	{
		string Name { get; set; }
		int XP { get; set; }
	}

	public interface IHealthChanged : IRootEvent
	{
		int Amount { get; }
	}

	public interface IXPGained : IRootEvent
	{
		int Amount { get; }
	}

	public interface IInventoryEvent : IRootEvent
	{
		MiniId InventoryId { get; }
	}

	public interface IInventoryAdded : IRootEvent, IInventoryEvent, IInventoryCountChanged
	{
		string Name { get; set; }
	}

	public interface IInventoryDropped : IInventoryEvent, IInventoryCountChanged
	{
		int Amount { get; set; }
	}

	public interface IInventoryCountChanged : IInventoryEvent
	{
		int Amount { get; set; }
	}

	public interface IFightEvent : IRootEvent
	{
		MiniId FightId { get; }
	}

	public interface IFightEntered : IFightEvent
	{
		
	}

	public interface IFightExited : IFightEvent
	{
		
	}

	#endregion

	#region Domain events

	[DataContract]
	internal class CreatedEvent : AggregateRootEventBase, ICreatedEvent, IAggregateRootCreatedEvent
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int XP { get; set; }
	}

	[DataContract]
	internal class HealthChanged : AggregateRootEventBase, IHealthChanged
	{
		[DataMember]
		public int Amount { get; }
	}

	[DataContract]
	internal class XPGained : AggregateRootEventBase, IXPGained
	{
		[DataMember]
		public int Amount { get; }
	}

	[DataContract]
	internal class InventoryAdded : AggregateRootEventBase, IInventoryAdded
	{
		[DataMember]
		public MiniId InventoryId { get; }

		[DataMember]
		public string Name { get; set; }

		public int Amount { get; set; }
	}

	[DataContract]
	internal class InventoryDropped : AggregateRootEventBase, IInventoryDropped
	{
		[DataMember]
		public MiniId InventoryId { get; set; }

		[DataMember]
		public int Amount { get; set; }
	}

	#endregion

	#region Event stream (service fabric specific)

	[DataContract]
	[KnownType(typeof(CreatedEvent))]
	[KnownType(typeof(HealthChanged))]
	[KnownType(typeof(XPGained))]
	[KnownType(typeof(InventoryAdded))]
	[KnownType(typeof(InventoryAdded))]
	[KnownType(typeof(InventoryAdded))]
	[KnownType(typeof(InventoryAdded))]
	internal class TheEventStream : DomainEventStreamBase
	{
	}

	#endregion

	#region Domain

	public class Domain : AggregateRoot<IRootEvent>
	{
		public Domain()
		{
			RegisterEventAppliers()
				.For<ICreatedEvent>(e =>
				{
					this.Name = e.Name;
					this.XP = e.XP;
					this.Health = 0;
				})
				.For<IHealthChanged>(e =>
				{
					this.Health += e.Amount;
				})
				.For<IXPGained>(e =>
				{
					this.XP += e.Amount;
				})
				.For<IInventoryAdded>(e =>
				{
					var inventory = this.Backpack.SingleOrDefault(c => c.Id.Equals(e.InventoryId));
					if (inventory == null)
					{
						inventory = new Inventory(this, e.InventoryId, e.Name);
						this.Backpack.Add(inventory);
					}
					inventory.ApplyEvent(e);
				})
				.For<IInventoryDropped>(e =>
				{
					var inventory = this.Backpack.SingleOrDefault(c => c.Id.Equals(e.InventoryId));
					if (inventory != null)
					{
						inventory.ApplyEvent(e);
					}
					else
					{
						throw new NotSupportedException($"Backpack does not contain {e.InventoryId}");
					}
				});
		}

		public string Name { get; private set; }

		public int XP { get; private set; }

		public int Health { get; private set; }

		public List<Inventory> Backpack { get; set; }

		public class Inventory : Entity<Domain, IInventoryEvent>
		{
			public Inventory(Domain aggregateRoot, MiniId id, string name) : base(aggregateRoot)
			{
				this.Id = id;
				this.Name = name;
				this.Count = 0;

				RegisterEventAppliers().For<IInventoryCountChanged>(e => this.Count += e.Amount);
			}

			public MiniId Id { get; }

			public string Name { get; private set; }

			public int Count { get; private set; }
		}

		public class Weapon : Inventory
		{

			public Weapon(Domain aggregateRoot, MiniId id, string name, int damage) : base(aggregateRoot, id, name)
			{
				Damage = damage;
			}
			public int Damage { get; }
		}

		public void Create(string characterName)
		{
			var createdEvent = new CreatedEvent() {AggregateRootId = this.AggregateRootId, Name = characterName};

			RaiseEvent(createdEvent);
		}

		public void Attack(MiniId fightId, MiniId characterId)
		{
			
		}
	}


	#endregion

	[StatePersistence(StatePersistence.Persisted)]
	internal class CharacterActor : EventStoredActor<Domain, TheEventStream>, ICharacterActor
	{ 	
		public CharacterActor(Microsoft.ServiceFabric.Actors.Runtime.ActorService actorService, ActorId actorId)
			: base(actorService, actorId)
		{
		}

		protected override Task OnActivateAsync()
		{
			/* Load Domain from EventStream */
			return base.OnActivateAsync();
		}

		public async Task Handle(ICharacterCommand command, CancellationToken cancellationToken)
		{
			await this.ExecuteCommandAsync(() =>
			{
				/* run Command on Domain */
			}, command, cancellationToken);

			//var stateKey = $"@@command_{command.CommandId}@@";

			//var commandHandlingState = await ExecutionHelper.ExecuteWithRetriesAsync(async (ct) => 
			//	await this.StateManager.TryGetStateAsync<CommandHandlingState>(stateKey, ct), 
			//	3, TimeSpan.FromSeconds(3), cancellationToken );

			//if (commandHandlingState.HasValue)
			//{
			//	if (commandHandlingState.Value.Handled == CommandHandlingState.CommandState.Handled)
			//	{
			//		// Duplicate command, already handled by us
			//		return;
			//	}
			//	else
			//	{
			//		// We recived it before but never handled it
			//	}
			//}

			///* run Command on Domain */
			//if (command is Interfaces.Commands.Create)
			//{
			//	Handle((Interfaces.Commands.Create) command);
			//}

			//await ExecutionHelper.ExecuteWithRetriesAsync(async (ct) =>
			//	await this.StateManager.SetStateAsync<CommandHandlingState>(stateKey, 
			//	new CommandHandlingState(){CommandId = command.CommandId, Handled = CommandHandlingState.CommandState.Handled}, ct),
			//	3, TimeSpan.FromSeconds(3), cancellationToken);

		}

		private void Handle(Interfaces.Commands.Create command)
		{
			DomainState.Create(command.Name);
		}

		private void Handle(Interfaces.Commands.Attack command)
		{
			DomainState.Attack(command.FightId, command.CharacterId);
		}


		public void Handle(IRootEvent rootEvent)
		{
			StoreDomainEventAsync(rootEvent);
		}
	}

	[DataContract]
	public class CommandHandlingState
	{
		[DataMember]
		public Guid CommandId { get; set; }

		public CommandState Handled { get; set; }

		public enum CommandState
		{
			Recived = 1,
			Handled = 2,
		}
	}
}
