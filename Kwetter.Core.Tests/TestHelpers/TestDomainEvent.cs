namespace Kwetter.Core.Tests.TestHelpers;

internal record TestDomainEvent : DomainEvent
{
    public TestDomainEvent(Guid guid) : base(guid)
    {
    }
}
