namespace EventApp.Api.Events;

public sealed record CreateEventRequest(
    string Name,
    string VenueAddress,
    string VenueName,
    int VenueCapacity,
    int ExpectedAttendees,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    string OwnerName,
    string OwnerLegalId
);
