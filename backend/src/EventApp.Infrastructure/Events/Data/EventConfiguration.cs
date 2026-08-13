using EventApp.Domain.Events.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.Infrastructure.Events.Data;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{

    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable(
            "events",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "ck_events_venue_capacity_positive",
                    "venue_capacity > 0");
                tableBuilder.HasCheckConstraint(
                    "ck_events_expected_attendees_non_negative",
                    "expected_attendees >= 0");
                tableBuilder.HasCheckConstraint(
                    "ck_events_expected_attendees_within_capacity",
                    "expected_attendees <= venue_capacity");
                tableBuilder.HasCheckConstraint(
                    "ck_events_end_date_after_start_date",
                    "end_date > start_date");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(Event.MaxNameLength)
            .IsRequired();

        builder.Property(x => x.VenueName)
            .HasColumnName("venue_name")
            .HasMaxLength(Event.MaxVenueNameLength)
            .IsRequired();

        builder.Property(x => x.VenueAddress)
            .HasColumnName("venue_address")
            .HasMaxLength(Event.MaxVenueAddressLength)
            .IsRequired();

        builder.Property(x => x.VenueCapacity)
            .HasColumnName("venue_capacity")
            .IsRequired();

        builder.Property(x => x.ExpectedAttendees)
            .HasColumnName("expected_attendees")
            .IsRequired();

        builder.Property(x => x.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(Event.MaxDescriptionLength)
            .IsRequired();

        builder.Property(x => x.OwnerName)
            .HasColumnName("owner_name")
            .HasMaxLength(Event.MaxOwnerNameLength)
            .IsRequired();

        builder.Property(x => x.OwnerLegalId)
            .HasColumnName("owner_legal_id")
            .HasMaxLength(Event.MaxOwnerLegalIdLength)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestamp with time zone");

        builder.Ignore(x => x.IsDeleted);

        builder.HasIndex(x => x.StartDate)
            .HasDatabaseName("ix_events_start_date");

        builder.HasIndex(x => x.OwnerLegalId)
            .HasDatabaseName("ix_events_owner_legal_id");

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }

}
