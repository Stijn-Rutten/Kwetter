using Kwetter.Core;
using Kwetter.Domain.Events;

namespace Kwetter.Domain.Commands;

public record PostKweet(string Content) : Command
{
    public KweetPosted MapToKweetPosted(Guid guid)
    {
        return new KweetPosted(guid, Content, DateTimeOffset.UtcNow);
    }
}