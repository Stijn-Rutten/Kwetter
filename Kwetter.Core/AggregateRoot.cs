namespace Kwetter.Core;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly List<DomainEvent> _events;

    public int Version { get; private set; }
    public int OriginalVersion { get; private set; }
    public bool IsReplaying { get; set; } = false;

    protected AggregateRoot(TId id) : base(id)
    {
        OriginalVersion = 0;
        Version = 0;
        _events = new List<DomainEvent>();
    }

    protected AggregateRoot(TId id, IEnumerable<DomainEvent> events) : this(id)
    {
        ReplayEvents(events);
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

    private void ReplayEvents(IEnumerable<DomainEvent> events)
    {
        IsReplaying = true;
        foreach (DomainEvent e in events)
        {
            When(e);
            OriginalVersion++;
            Version++;
        }
        IsReplaying = false;
    }
}