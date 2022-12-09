namespace Kwetter.Infrastructure.Entities;

internal record EventStoreMessage(
    Guid Id,
    Guid AggregateId,
    string MessageType,
    string EventData,
    DateTimeOffset CreatedAt
);