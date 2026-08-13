namespace EventApp.Application.Events.Queries.GetEvents;

public sealed record GetEventItem(
    Guid Id,
    string Name,
    string VenueAddress,
    string VenueName,
    int VenueCapacity,
    int ExpectedAttendees,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    string OwnerName,
    string OwnerLegalId,
    string Status,
    DateTimeOffset CreatedAt
);
