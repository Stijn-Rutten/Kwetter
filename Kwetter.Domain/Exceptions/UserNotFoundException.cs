using Kwetter.Domain.ValueObjects;

namespace Kwetter.Domain.Exceptions;
public class UserNotFoundException : Exception
{
    public UserId UserId { get; }

    public UserNotFoundException(UserId userId) : base($"User with id {userId.Value} not found.")
    {
        UserId = userId;
    }
}
