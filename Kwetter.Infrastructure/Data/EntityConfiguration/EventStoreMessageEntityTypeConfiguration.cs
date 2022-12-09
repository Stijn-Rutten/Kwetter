using Kwetter.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kwetter.Infrastructure.Data.EntityConfiguration;

internal class EventStoreMessageEntityTypeConfiguration : IEntityTypeConfiguration<EventStoreMessage>
{
    public void Configure(EntityTypeBuilder<EventStoreMessage> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.MessageType).IsRequired();
        builder.Property(e => e.AggregateId).IsRequired();
        builder.Property(e => e.EventData).IsRequired();
        builder.Property(e => e.CreatedAt).ValueGeneratedOnAdd();
    }
}