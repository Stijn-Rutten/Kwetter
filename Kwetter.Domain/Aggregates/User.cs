using Kwetter.Core;
using Kwetter.Domain.ValueObjects;

namespace Kwetter.Domain.Aggregates;

public class User : AggregateRoot<UserId>
{
    public List<Kweet> Kweets { get; private set; } = new List<Kweet>();

    public User(UserId id) : base(id) { }
    public User(UserId id, IEnumerable<DomainEvent> events) : base(id, events) { }

    public IEnumerable<Kweet> GetKweets()
    {
        throw new NotImplementedException();
    }

    protected override void When(dynamic @event)
    {
        throw new NotImplementedException();
    }
}
