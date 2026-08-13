namespace EventApp.Application.Events.Commands.CreateEvent;

public sealed record CreateEventCommand(
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
