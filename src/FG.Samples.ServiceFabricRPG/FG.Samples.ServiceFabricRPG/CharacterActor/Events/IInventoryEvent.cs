using FG.Common.Utils;

namespace CharacterActor
{
	public interface IInventoryEvent : IRootEvent
	{
		MiniId InventoryId { get; }
	}
}