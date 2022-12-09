using Kwetter.Core;
using Kwetter.Domain.Aggregates;
using Kwetter.Domain.ValueObjects;
using Kwetter.Infrastructure.Data;
using Kwetter.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kwetter.Infrastructure.Repositories;

internal class UserEventSourceRepository : IEventSourceRepository<UserId, User>
{
    private readonly KwetterDbContext _dbContext;

    public UserEventSourceRepository(KwetterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByIdAsync(UserId id)
    {
        // Query all the events from the event store with the associated aggregate Value
        var events = await _dbContext.Messages
            .Where(m => m.AggregateId == id.Value)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();

        var domainEvents = events.Select(e => DeserializeEventData(e.MessageType, e.EventData));
      
        var user = new User(id, domainEvents);

        return user;
    }

    public async Task SaveAsync(User user)
    {
        IEnumerable<DomainEvent> domainEvents = user.GetEvents();

        // Map domain events to generic event
        var events = domainEvents.Select(e => SerializeDomainEvent(user.Id, e));

        foreach (var e in events)
        {
            await AddIfNotExistsAsync(e);
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddIfNotExistsAsync(EventStoreMessage messageToAdd)
    {
        var exists = await _dbContext.Messages.AnyAsync(x => x.Id == messageToAdd.Id);

        if (!exists)
        {
            await _dbContext.Messages.AddAsync(messageToAdd);
        }
    }

    private static EventStoreMessage SerializeDomainEvent(UserId userId, DomainEvent @event)
    {
        var serializedEvent = JsonConvert.SerializeObject(@event);

        return new EventStoreMessage(@event.Id, userId.Value, @event.GetType().ToString(), serializedEvent, DateTimeOffset.Now);
    }

    private static DomainEvent DeserializeEventData(string messageType, string eventData)
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

