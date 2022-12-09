namespace Kwetter.Core.Tests.TestHelpers;

internal class AggregateRootUnderTest : AggregateRoot<int>
{

    public bool DomainEventHandled { get; private set; } = false;
    public AggregateRootUnderTest(int id) : base(id)
    {
    }

    public AggregateRootUnderTest(int id, IEnumerable<DomainEvent> events) : base(id, events) { }

    public void ExecuteCommand()
    {
        RaiseEvent(new TestDomainEvent(Guid.NewGuid()));
    }

    protected override void When(dynamic @event)
    {
        DomainEventHandled = true;
    }
}