using EventApp.Domain.Events.Entities;

namespace EventApp.Application.Events;

public sealed record EventPage(
    IReadOnlyList<Event> Items,
    int TotalCount);
