namespace Kwetter.Core;

public abstract record DomainEvent
{
	public Guid Id { get; }

	public DomainEvent(Guid guid)
	{
		Id = guid;
	}
};