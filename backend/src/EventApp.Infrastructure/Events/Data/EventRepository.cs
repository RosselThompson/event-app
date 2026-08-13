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

    public async Task<EventPage> GetAllAsync(int page, int pageSize, string? name, DateTimeOffset? startDateFrom, DateTimeOffset? startDateTo, CancellationToken cancellationToken)
    {
        IQueryable<Event> query = dbContext.Events.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name))
        {
            string searchTerm = name.Trim();

            query = query.Where(eventItem =>
                EF.Functions.ILike(
                    eventItem.Name,
                    $"%{searchTerm}%"));
        }
        if (startDateFrom.HasValue)
        {
            query = query.Where(eventItem =>
            eventItem.StartDate >= startDateFrom.Value);
        }
        if (startDateTo.HasValue)
        {
            query = query.Where(eventItem =>
            eventItem.StartDate <= startDateTo.Value);
        }

        int totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<Event> items = await query
        .OrderBy(eventItem => eventItem.StartDate)
        .ThenBy(eventItem => eventItem.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

        return new EventPage(items, totalCount);
    }

    public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
