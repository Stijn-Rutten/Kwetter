using FluentAssertions;
using Kwetter.Core.Tests.TestHelpers;

namespace Kwetter.Core.Tests;

public class AggregateRootTests
{
    public class Constructors
    {
        [Fact]
        public void Test_Default_Constructor()
        {
            // Arrange
            int id = 1;

            // Act
            var sut = new AggregateRootUnderTest(id);

            // Assert
            sut.OriginalVersion.Should().Be(0);
            sut.Version.Should().Be(0);
        }

        [Fact]
        //[MemberData(nameof(DomainEventsHelpers.GetDomainEvents), MemberType = typeof(DomainEventsHelpers)]
        public void Test_Constructor_With_Empty_Events_List()
        {
            // Arrange
            int id = 1;
            var events = new List<DomainEvent> { };

            // Act
            var sut = new AggregateRootUnderTest(id, events);

            // Assert
            sut.OriginalVersion.Should().Be(0);
            sut.Version.Should().Be(0);
        }

        [Fact]
        public void Test_Constructor_With_Populated_Events_List()
        {
            // Arrange
            int id = 1;
            var events = new List<DomainEvent> {
                new TestDomainEvent(Guid.NewGuid()),
                new TestDomainEvent(Guid.NewGuid()),
                new TestDomainEvent(Guid.NewGuid()),
                new TestDomainEvent(Guid.NewGuid()),
            };

            // Act
            var sut = new AggregateRootUnderTest(id, events);

            // Assert
            sut.OriginalVersion.Should().Be(4);
            sut.Version.Should().Be(4);
        }
    }

    public class RaiseEvent {
        
        [Fact]
        public void RaiseEvent_Should_Increment_Version()
        {
            // arrange
            int id = 0;
            var sut = new AggregateRootUnderTest(id);

            sut.Version.Should().Be(0);
            sut.Version.Should().Be(0);


            // Act
            sut.ExecuteCommand();

            // Assert

            var events = sut.GetEvents();

            events.Count().Should().Be(1);
            sut.Version.Should().Be(1);
            sut.OriginalVersion.Should().Be(0);
            sut.DomainEventHandled.Should().BeTrue();
        }      
    }

    public class ClearEvents
    {
        [Fact]
        public void ClearEvents_Should_Clear()
        {
            var events = new List<DomainEvent>
            {
                new TestDomainEvent(Guid.NewGuid()),
                new TestDomainEvent(Guid.NewGuid())
            };
            var id = 1;
            var sut = new AggregateRootUnderTest(id, events);

            sut.ClearEvents();

            var result = sut.GetEvents();

            result.Count().Should().Be(0);
        }
    }
}
