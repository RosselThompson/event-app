using EventApp.Domain.Events.Entities;

namespace EventApp.Application.Events.Queries.GetEventById;


public sealed class GetEventByIdHandler(IEventRepository repository)
{
    public async Task<GetEventByIdResult?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        Event? eventItem = await repository.GetByIdAsync(query.EventId, cancellationToken);

        return eventItem is null ? null : new GetEventByIdResult(eventItem.Id, eventItem.Name,
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
