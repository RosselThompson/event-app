using EventApp.Application.Events;
using EventApp.Domain.Events.Entities;

namespace EventApp.Infrastructure.Events.Data;

public sealed class EventRepository(EventsDbContext dbContext) : IEventRepository
{
    public void Add(Event eventItem)
    {
        dbContext.Events.Add(eventItem);
    }

}
