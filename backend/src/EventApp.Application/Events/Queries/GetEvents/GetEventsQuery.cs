namespace EventApp.Application.Events.Queries.GetEvents;

public sealed record GetEventsQuery(
    int Page,
    int PageSize,
    string? Name,
    DateTimeOffset? StartDateFrom,
    DateTimeOffset? StartDateTo
);
