using FG.Common.Utils;

namespace CharacterActor
{
	public interface IFightEvent : IRootEvent
	{
		MiniId FightId { get; }
	}
}