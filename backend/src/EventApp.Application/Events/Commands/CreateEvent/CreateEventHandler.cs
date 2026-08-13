using EventApp.Application.Events;
using EventApp.Application.Persistence;
using EventApp.Domain.Events.Entities;

namespace EventApp.Application.Events.Commands.CreateEvent;

public sealed class CreateEventHandler(
    IEventRepository repository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider
)
{
    public async Task<CreateEventResult> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {

        DateTimeOffset currentTime =
          timeProvider.GetUtcNow();

        Event eventItem = Event.Create(
           command.Name,
        command.VenueAddress,
        command.VenueName,
        command.VenueCapacity,
        command.ExpectedAttendees,
        command.StartDate,
        command.EndDate,
        command.Description,
        command.OwnerName,
        command.OwnerLegalId,
        currentTime
        );

        repository.Add(eventItem);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateEventResult(eventItem.Id);
    }
}
