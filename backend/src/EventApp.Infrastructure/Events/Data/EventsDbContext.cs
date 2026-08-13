using Microsoft.EntityFrameworkCore;

namespace EventApp.Infrastructure.Events.Data;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("events");

        base.OnModelCreating(modelBuilder);
    }
}


