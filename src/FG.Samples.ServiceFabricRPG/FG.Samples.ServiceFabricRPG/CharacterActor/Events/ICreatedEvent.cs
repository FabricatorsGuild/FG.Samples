namespace CharacterActor
{
	public interface ICreatedEvent : IRootEvent
	{
		string Name { get; set; }
		int XP { get; set; }
	}
}