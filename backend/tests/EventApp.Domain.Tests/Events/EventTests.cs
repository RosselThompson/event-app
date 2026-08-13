using EventApp.Domain.Common;
using EventApp.Domain.Events.Entities;
using EventApp.Domain.Events.Enums;

namespace EventApp.Domain.Tests.Events;

public sealed class EventTests
{
    private static readonly DateTimeOffset CurrentTime =
        new(2026, 8, 13, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Create_WithValidData_ShouldCreateDraftEvent()
    {
        Event eventItem = CreateValidEvent();

        Assert.NotEqual(Guid.Empty, eventItem.Id);
        Assert.Equal("Tech Conference", eventItem.Name);
        Assert.Equal("Convention Center", eventItem.VenueName);
        Assert.Equal(EventStatus.Draft, eventItem.Status);
        Assert.Equal(CurrentTime, eventItem.CreatedAt);
        Assert.Null(eventItem.UpdatedAt);
        Assert.Null(eventItem.DeletedAt);
        Assert.False(eventItem.IsDeleted);
    }

    [Fact]
    public void Create_ShouldTrimTextValues()
    {
        Event eventItem = CreateValidEvent(
            name: "  Tech Conference  ",
            venueName: "  Convention Center  ");

        Assert.Equal("Tech Conference", eventItem.Name);
        Assert.Equal("Convention Center", eventItem.VenueName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrow(string? name)
    {
        Assert.Throws<DomainException>(() =>
            CreateValidEvent(name: name!));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidCapacity_ShouldThrow(int capacity)
    {
        Assert.Throws<DomainException>(() =>
            CreateValidEvent(venueCapacity: capacity));
    }

    [Fact]
    public void Create_WhenExpectedAttendeesExceedCapacity_ShouldThrow()
    {
        Assert.Throws<DomainException>(() =>
            CreateValidEvent(venueCapacity: 100, expectedAttendees: 101));
    }

    [Fact]
    public void Create_WhenStartDateIsNotInFuture_ShouldThrow()
    {
        Assert.Throws<DomainException>(() =>
            CreateValidEvent(startDate: CurrentTime));
    }

    [Fact]
    public void Create_WhenEndDateIsNotAfterStartDate_ShouldThrow()
    {
        DateTimeOffset startDate = CurrentTime.AddDays(1);

        Assert.Throws<DomainException>(() =>
            CreateValidEvent(startDate: startDate, endDate: startDate));
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateValuesAndAuditDate()
    {
        Event eventItem = CreateValidEvent();
        DateTimeOffset updatedAt = CurrentTime.AddHours(1);

        eventItem.Update(
            "Updated Conference",
            "New Address",
            "New Venue",
            600,
            500,
            CurrentTime.AddDays(2),
            CurrentTime.AddDays(3),
            "Updated description",
            "Updated Owner",
            "OWNER-002",
            updatedAt);

        Assert.Equal("Updated Conference", eventItem.Name);
        Assert.Equal(600, eventItem.VenueCapacity);
        Assert.Equal(updatedAt, eventItem.UpdatedAt);
    }

    [Fact]
    public void Delete_WithoutAssociatedAttendees_ShouldSoftDeleteEvent()
    {
        Event eventItem = CreateValidEvent();
        DateTimeOffset deletedAt = CurrentTime.AddHours(1);

        eventItem.Delete(hasAssociatedAttendees: false, deletedAt);

        Assert.True(eventItem.IsDeleted);
        Assert.Equal(deletedAt, eventItem.DeletedAt);
    }

    [Fact]
    public void Delete_WithAssociatedAttendees_ShouldThrow()
    {
        Event eventItem = CreateValidEvent();

        Assert.Throws<DomainException>(() =>
            eventItem.Delete(
                hasAssociatedAttendees: true,
                CurrentTime.AddHours(1)));
    }

    [Fact]
    public void Delete_WhenAlreadyDeleted_ShouldThrow()
    {
        Event eventItem = CreateValidEvent();
        eventItem.Delete(false, CurrentTime.AddHours(1));

        Assert.Throws<DomainException>(() =>
            eventItem.Delete(false, CurrentTime.AddHours(2)));
    }

    [Fact]
    public void Update_WhenEventIsDeleted_ShouldThrow()
    {
        Event eventItem = CreateValidEvent();
        eventItem.Delete(false, CurrentTime.AddHours(1));

        Assert.Throws<DomainException>(() =>
            eventItem.Update(
                "Updated Conference",
                "New Address",
                "New Venue",
                600,
                500,
                CurrentTime.AddDays(2),
                CurrentTime.AddDays(3),
                "Updated description",
                "Updated Owner",
                "OWNER-002",
                CurrentTime.AddHours(2)));
    }

    private static Event CreateValidEvent(
        string name = "Tech Conference",
        string venueAddress = "123 Main Street",
        string venueName = "Convention Center",
        int venueCapacity = 500,
        int expectedAttendees = 400,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null)
    {
        DateTimeOffset effectiveStartDate = startDate ?? CurrentTime.AddDays(1);
        DateTimeOffset effectiveEndDate = endDate ?? effectiveStartDate.AddHours(8);

        return Event.Create(
            name,
            venueAddress,
            venueName,
            venueCapacity,
            expectedAttendees,
            effectiveStartDate,
            effectiveEndDate,
            "Technology conference",
            "Example Organization",
            "OWNER-001",
            CurrentTime);
    }
}
