using Microsoft.EntityFrameworkCore;
using Kwetter.Infrastructure.Data.EntityConfiguration;
using Kwetter.Infrastructure.Entities;

namespace Kwetter.Infrastructure.Data;

public class KwetterDbContext : DbContext
{
    public virtual DbSet<EventStoreMessage> Messages { get; set; }

    public KwetterDbContext()
    {

    }

    public KwetterDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new EventStoreMessageEntityTypeConfiguration().Configure(modelBuilder.Entity<EventStoreMessage>());
    }
}
