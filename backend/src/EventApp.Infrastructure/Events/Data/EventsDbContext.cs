using Microsoft.EntityFrameworkCore;
using EventApp.Domain.Events.Entities;
using EventApp.Application.Persistence;

namespace EventApp.Infrastructure.Events.Data;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Event> Events => Set<Event>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("events");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}


