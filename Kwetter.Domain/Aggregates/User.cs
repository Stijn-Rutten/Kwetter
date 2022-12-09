using Kwetter.Core;
using Kwetter.Domain.Commands;
using Kwetter.Domain.Events;
using Kwetter.Domain.ValueObjects;

namespace Kwetter.Domain.Aggregates;

public class User : AggregateRoot<UserId>
{
    private readonly List<Kweet> _kweets = new List<Kweet>();

    public User(UserId id) : base(id) { }
    public User(UserId id, IEnumerable<DomainEvent> events) : base(id, events) { }

    public IEnumerable<Kweet> GetKweets(int size, int skip)
    {
        return _kweets.Skip(skip).Take(size);
    }

    public void PostKweet(PostKweet command)
    {
        // TODO: Validate Business Rules

        var e = command.MapToKweetPosted(Guid.NewGuid());

        RaiseEvent(e);
    }

    protected override void When(dynamic @event)
    {
        Handle(@event);

    }

    private void Handle(KweetPosted e)
    {
        var kweet = new Kweet(e.Id);
        kweet.Post(e.Content, Id, e.PostedAt);
        _kweets.Add(kweet);
    }
}
