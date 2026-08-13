using EventApp.Domain.Common;
using EventApp.Domain.Events.Enums;

namespace EventApp.Domain.Events.Entities;

public sealed class Event : AuditableEntity
{
    public const int MaxNameLength = 200;
    public const int MaxVenueNameLength = 200;
    public const int MaxVenueAddressLength = 300;
    public const int MaxDescriptionLength = 2_000;
    public const int MaxOwnerNameLength = 200;
    public const int MaxOwnerLegalIdLength = 100;

    private Event() { }

    private Event(Guid id, string name, string venueAddress, string venueName, int venueCapacity, int expectedAttendees, DateTimeOffset startDate, DateTimeOffset endDate, string description, string ownerName, string ownerLegalId, EventStatus eventStatus, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        VenueAddress = venueAddress;
        VenueName = venueName;
        VenueCapacity = venueCapacity;
        ExpectedAttendees = expectedAttendees;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        OwnerName = ownerName;
        OwnerLegalId = ownerLegalId;
        Status = eventStatus;
        CreatedAt = createdAt;
    }
    public string Name { get; private set; } = string.Empty;
    public string VenueName { get; private set; } = string.Empty;
    public string VenueAddress { get; private set; } = string.Empty;
    public int VenueCapacity { get; private set; } = 0;
    public int ExpectedAttendees { get; private set; } = 0;
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string OwnerName { get; private set; } = string.Empty;
    public string OwnerLegalId { get; private set; } = string.Empty;
    public EventStatus Status { get; private set; } = EventStatus.Draft;

    public static Event Create(string name, string venueAddress, string venueName, int venueCapacity, int expectedAttendees, DateTimeOffset startDate, DateTimeOffset endDate, string description, string ownerName, string ownerLegalId, DateTimeOffset createdAt)
    {
        Validate(name, venueAddress, venueName, venueCapacity, expectedAttendees, startDate, endDate, description, ownerName, ownerLegalId, createdAt);

        return new Event(Guid.NewGuid(), name.Trim(), venueAddress.Trim(), venueName.Trim(), venueCapacity, expectedAttendees, startDate, endDate, description.Trim(), ownerName.Trim(), ownerLegalId.Trim(), EventStatus.Draft, createdAt);
    }

    public void Update(string name, string venueAddress, string venueName, int venueCapacity, int expectedAttendees, DateTimeOffset startDate, DateTimeOffset endDate, string description, string ownerName, string ownerLegalId, DateTimeOffset updatedAt)
    {
        Validate(name, venueAddress, venueName, venueCapacity, expectedAttendees, startDate, endDate, description, ownerName, ownerLegalId, updatedAt);

        MarkAsUpdated(updatedAt);

        Name = name.Trim();
        VenueAddress = venueAddress.Trim();
        VenueName = venueName.Trim();
        VenueCapacity = venueCapacity;
        ExpectedAttendees = expectedAttendees;
        StartDate = startDate;
        EndDate = endDate;
        Description = description.Trim();
        OwnerName = ownerName.Trim();
        OwnerLegalId = ownerLegalId.Trim();
    }

    public void Delete(bool hasAssociatedAttendees, DateTimeOffset deletedAt)
    {
        if (hasAssociatedAttendees)
        {
            throw new DomainException("An event with associated attendees cannot be deleted.");
        }

        MarkAsDeleted(deletedAt);
    }

    private static void Validate(
        string name,
        string venueAddress,
        string venueName,
        int venueCapacity,
        int expectedAttendees,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string ownerName,
        string ownerLegalId,
        DateTimeOffset currentTime)
    {
        ValidateRequiredText(name, nameof(name), MaxNameLength);
        ValidateRequiredText(venueName, nameof(venueName), MaxVenueNameLength);
        ValidateRequiredText(venueAddress, nameof(venueAddress), MaxVenueAddressLength);
        ValidateRequiredText(ownerName, nameof(ownerName), MaxOwnerNameLength);
        ValidateRequiredText(ownerLegalId, nameof(ownerLegalId), MaxOwnerLegalIdLength);
        ValidateOptionalText(description, nameof(description), MaxDescriptionLength);

        if (venueCapacity <= 0)
        {
            throw new DomainException("Venue capacity must be greater than zero.");
        }

        if (expectedAttendees < 0)
        {
            throw new DomainException("Expected attendees cannot be negative.");
        }

        if (expectedAttendees > venueCapacity)
        {
            throw new DomainException("Expected attendees cannot exceed venue capacity.");
        }

        if (startDate <= currentTime)
        {
            throw new DomainException("Event start date must be in the future.");
        }

        if (endDate <= startDate)
        {
            throw new DomainException("Event end date must be later than its start date.");
        }
    }

    private static void ValidateRequiredText(string value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{fieldName} is required.");
        }

        if (value.Trim().Length > maxLength)
        {
            throw new DomainException($"{fieldName} cannot exceed {maxLength} characters.");
        }
    }

    private static void ValidateOptionalText(string value, string fieldName, int maxLength)
    {
        if (value is null)
        {
            throw new DomainException($"{fieldName} cannot be null.");
        }

        if (value.Trim().Length > maxLength)
        {
            throw new DomainException($"{fieldName} cannot exceed {maxLength} characters.");
        }
    }

}
