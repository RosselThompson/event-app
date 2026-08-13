namespace EventApp.Api.Events;

public sealed record GetEventsRequest(
    int Page = 1,
    int PageSize = 20,
    string? Name = null,
    DateTimeOffset? StartDateFrom = null,
    DateTimeOffset? StartDateTo = null);
