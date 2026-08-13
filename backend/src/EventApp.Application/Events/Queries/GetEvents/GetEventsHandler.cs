using EventApp.Application.Common;
using EventApp.Domain.Events.Entities;

namespace EventApp.Application.Events.Queries.GetEvents;

public sealed class GetEventsHandler(IEventRepository repository)
{
    public async Task<PagedResult<GetEventItem>> Handle(GetEventsQuery query, CancellationToken cancellationToken)
    {
        EventPage eventPage = await repository.GetAllAsync(query.Page,
                query.PageSize,
                query.Name,
                query.StartDateFrom,
                query.StartDateTo,
                cancellationToken);

        IReadOnlyList<GetEventItem> items =
           eventPage.Items
               .Select(Map)
               .ToList();

        return new PagedResult<GetEventItem>(items, query.Page, query.PageSize, eventPage.TotalCount);
    }

    private static GetEventItem Map(Event eventItem)
    {
        return new GetEventItem(
            eventItem.Id,
            eventItem.Name,
            eventItem.VenueAddress,
            eventItem.VenueName,
            eventItem.VenueCapacity,
            eventItem.ExpectedAttendees,
            eventItem.StartDate,
            eventItem.EndDate,
            eventItem.Description,
            eventItem.OwnerName,
            eventItem.OwnerLegalId,
            eventItem.Status.ToString(),
            eventItem.CreatedAt);
    }
}

