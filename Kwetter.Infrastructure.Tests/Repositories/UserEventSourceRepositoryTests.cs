using FluentAssertions;
using Kwetter.Domain.Events;
using Kwetter.Domain.Exceptions;
using Kwetter.Domain.ValueObjects;
using Kwetter.Infrastructure.Data;
using Kwetter.Infrastructure.Entities;
using Kwetter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kwetter.Infrastructure.Tests.Repositories;
public class UserEventSourceRepositoryTests
{
    public class GetByIdAsync
    {
        // TODO: Refactor this test.
        //[Fact]
        //public async Task ShouldNotReturnUserIfNotExists()
        //{
        //    // Arrange
        //    var userId = new UserId(Guid.NewGuid());
        //    var notUserId = new UserId(Guid.NewGuid());

        //    var events = new List<EventStoreMessage> {

        //        new EventStoreMessage(Guid.NewGuid(), notUserId.Value, typeof(KweetPosted).ToString(), null, new DateTimeOffset())
        //    };

        //    Mock<KwetterDbContext> mockContext = SetupQueryableKwetterDbContext(events);

        //    var sut = new UserEventSourceRepository(mockContext.Object);

        //    // Act
        //    Func<Task> act = async () => await sut.GetByIdAsync(userId);

        //    // Assert
        //    await act.Should().ThrowAsync<UserNotFoundException>().WithMessage("User with id " + userId.Value + " not found.");
        //}

        private static Mock<KwetterDbContext> SetupQueryableKwetterDbContext(List<EventStoreMessage> events)
        {
            var queryableEvents = events.AsQueryable();

            var mockSet = new Mock<DbSet<EventStoreMessage>>();
            
            mockSet.As<IDbAsyncEnumerable<EventStoreMessage>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<EventStoreMessage>(queryableEvents.GetEnumerator()));
            
            mockSet.As<IQueryable<EventStoreMessage>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<EventStoreMessage>(queryableEvents.Provider));

            mockSet.As<IQueryable<EventStoreMessage>>().Setup(m => m.Expression).Returns(queryableEvents.Expression);
            mockSet.As<IQueryable<EventStoreMessage>>().Setup(m => m.ElementType).Returns(queryableEvents.ElementType);
            mockSet.As<IQueryable<EventStoreMessage>>().Setup(m => m.GetEnumerator()).Returns(() => queryableEvents.GetEnumerator());

            var mockContext = new Mock<KwetterDbContext>();
            mockContext.Setup(m => m.Messages).Returns(mockSet.Object);
            
            return mockContext;
        }
    }
}

internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestDbAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestDbAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestDbAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute(expression));
    }

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute<TResult>(expression));
    }
}

internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
{
    public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    { }

    public TestDbAsyncEnumerable(Expression expression)
        : base(expression)
    { }

    public IDbAsyncEnumerator<T> GetAsyncEnumerator()
    {
        return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
    {
        return GetAsyncEnumerator();
    }

    IQueryProvider IQueryable.Provider
    {
        get { return new TestDbAsyncQueryProvider<T>(this); }
    }
}

internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestDbAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public void Dispose()
    {
        _inner.Dispose();
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_inner.MoveNext());
    }

    public T Current
    {
        get { return _inner.Current; }
    }

    object IDbAsyncEnumerator.Current
    {
        get { return Current; }
    }
}