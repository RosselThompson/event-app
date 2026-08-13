using EventApp.Application.Events;
using EventApp.Domain.Events.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.Infrastructure.Events.Data;

public sealed class EventRepository(EventsDbContext dbContext) : IEventRepository
{
    public void Add(Event eventItem)
    {
        dbContext.Events.Add(eventItem);
    }

    public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
