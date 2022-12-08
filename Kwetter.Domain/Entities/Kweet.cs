using Kwetter.Core;

namespace Kwetter.Domain.ValueObjects;

public class Kweet : Entity<Guid>
{
    public string? Content { get; private set; }
    public UserId? AuthorId { get; private set; }
    public DateTimeOffset PostedAt { get; private set; }

    public Kweet(Guid id) : base(id)
    {
    }

    public void Post(string Content, UserId authorId)
    {
        this.Content = Content;
        this.AuthorId = authorId;
        this.PostedAt = DateTimeOffset.UtcNow;
    }
}
