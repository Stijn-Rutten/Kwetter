namespace Kwetter.Infrastructure.Entities;

public record EventStoreMessage(
    Guid Id,
    Guid AggregateId,
    string MessageType,
    string EventData,
    DateTimeOffset CreatedAt
);