namespace CharacterActor
{
	public interface IInventoryAdded : IRootEvent, IInventoryEvent, IInventoryCountChanged
	{
		string Name { get; set; }
	}
}