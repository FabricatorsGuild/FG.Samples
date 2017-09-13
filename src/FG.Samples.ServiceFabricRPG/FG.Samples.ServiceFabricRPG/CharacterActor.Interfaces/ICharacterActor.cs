using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FG.Common.Utils;
using FG.CQRS;
using Microsoft.ServiceFabric.Actors;

namespace CharacterActor.Interfaces
{
	#region Commands

	namespace Commands
	{
		public class Create : CommandBase, ICharacterCommand
		{
			public string Name { get; set; }
		}

		public class EnterFight : CommandBase, ICharacterCommand
		{
			public MiniId FightId { get; set; }
		}

		public class ExitFight : CommandBase, ICharacterCommand
		{
			protected MiniId FightId { get; set; }
		}

		public enum ExitFightMode
		{
			Unknown = 0, // This should not happen, it's an error
			Won = 1,
			RunAwayScreaming = 2,
			Defeated = 3,
		}

		public class Attack :CommandBase, ICharacterCommand
		{
			public MiniId FightId { get; set; }
			public MiniId CharacterId { get; set; }
		}

		public class Attacked : CommandBase, ICharacterCommand
		{
			public MiniId FightId { get; set; }
			public MiniId CharacterId { get; set; }
		}
	}

	#endregion

	public interface ICharacterCommand : ICommand
	{		
	}

	public interface ICharacterActor : IActor
	{
		Task Handle(ICharacterCommand command, CancellationToken cancellationToken);
	}
}
