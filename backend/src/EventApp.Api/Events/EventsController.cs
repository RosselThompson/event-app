using EventApp.Application.Events.Commands.CreateEvent;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Api.Events;

[ApiController]
[Route("api/v1/events")]
public sealed class EventsController(CreateEventHandler createEventHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateEventResult>> Create(
        CreateEventRequest request, CancellationToken cancellationToken
    )
    {
        CreateEventCommand command = new CreateEventCommand(
            request.Name,
            request.VenueAddress,
            request.VenueName,
            request.VenueCapacity,
            request.ExpectedAttendees,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.OwnerName,
            request.OwnerLegalId
        );

        CreateEventResult result = await createEventHandler.Handle(
                command,
                cancellationToken);



        return CreatedAtAction(
            nameof(GetById),
            new { eventId = result.Id },
            result);
    }

    [HttpGet("{eventId:guid}")]
    public IActionResult GetById(Guid eventId)
    {
        return StatusCode(
            StatusCodes.Status501NotImplemented);
    }
}
