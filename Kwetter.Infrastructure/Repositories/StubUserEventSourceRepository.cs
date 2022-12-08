using Kwetter.Core;
using Kwetter.Domain.Aggregates;
using Kwetter.Domain.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Kwetter.Infrastructure.Repositories;

internal class StubUserEventSourceRepository : IEventSourceRepository<UserId, User>
{
    public async Task<User> GetByIdAsync(UserId id)
    {
        // Representation of a generic event in the event store. Always make sure these are ordere by version
        List<AggregateEvent> aggregateEvents = new();

        // Representation of the aggregate events from the event store as domain event. These will be used to build up the aggregate.
        IEnumerable<DomainEvent> domainEvents = aggregateEvents.Select(aggregateEvent => DeserializeEventData(aggregateEvent.MessageType, aggregateEvent.EventData));

        User user = new User(id, domainEvents);
        return user;
    }

    private DomainEvent DeserializeEventData(string messageType, string eventData)
    {
        Type? eventType = Type.GetType($"{nameof(Domain.Events)}.{messageType}");

        if (eventType is null)
        {
            throw new NotImplementedException();
        }

        JObject? obj = JsonConvert.DeserializeObject<JObject>(eventData);
        return obj?.ToObject(eventType) as DomainEvent ?? throw new NotImplementedException();
    }
}

internal record AggregateEvent(string Id, int Version, DateTimeOffset Timestamp, string MessageType, string EventData);