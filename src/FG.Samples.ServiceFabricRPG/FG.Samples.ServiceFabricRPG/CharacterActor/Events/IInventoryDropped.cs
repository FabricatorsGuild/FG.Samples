namespace CharacterActor
{
	public interface IInventoryDropped : IInventoryEvent, IInventoryCountChanged
	{
		int Amount { get; set; }
	}
}