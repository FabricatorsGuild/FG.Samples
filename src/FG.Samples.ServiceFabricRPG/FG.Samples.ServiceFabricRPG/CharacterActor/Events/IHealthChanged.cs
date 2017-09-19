namespace CharacterActor
{
	public interface IHealthChanged : IRootEvent
	{
		int Amount { get; }
	}
}