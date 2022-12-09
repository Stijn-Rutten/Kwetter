using Kwetter.Core;

namespace Kwetter.Domain.Events;

public record KweetPosted : DomainEvent
{ 
    public string Content { get; set; }
    public DateTimeOffset PostedAt { get; set; }

    public KweetPosted(Guid id, string content, DateTimeOffset postedAt) : base(id)
    {
        Content = content;
        PostedAt = postedAt;
    }
}
