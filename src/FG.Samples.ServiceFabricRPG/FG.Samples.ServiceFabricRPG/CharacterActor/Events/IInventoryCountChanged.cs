namespace CharacterActor
{
	public interface IInventoryCountChanged : IInventoryEvent
	{
		int Amount { get; set; }
	}
}