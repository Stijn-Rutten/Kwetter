namespace Kwetter.Core;

public interface IEventSourceRepository<TId, T> 
    where TId : ValueObject 
    where T : AggregateRoot<TId>
{
    Task<T> GetByIdAsync(TId id);
    Task SaveAsync(T entity);
}
