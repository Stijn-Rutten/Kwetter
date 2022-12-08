namespace Kwetter.Core;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly List<DomainEvent> _events;

    public int Version { get; private set; }
    public int OriginalVersion { get; private set; }
    public bool IsReplaying { get; set; } = false;

    public AggregateRoot(TId id) : base(id)
    {
        OriginalVersion = 0;
        Version = 0;
        _events = new List<DomainEvent>();
    }

    public AggregateRoot(TId id, IEnumerable<DomainEvent> events) : this(id)
    {
        IsReplaying = true;
        foreach(DomainEvent e in events)
        {
            When(e);
            OriginalVersion++;
            Version++;
        }
        IsReplaying = false;
    }

    public IEnumerable<DomainEvent> GetEvents()
    {
        return _events;
    }

    protected void RaiseEvent(DomainEvent @event)
    {
        When(@event);

        _events.Add(@event);
        Version++;
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected abstract void When(dynamic @event);
}

public class DomainEvent
{
}