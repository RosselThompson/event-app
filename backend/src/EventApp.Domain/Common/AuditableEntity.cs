namespace EventApp.Domain.Common;

public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedAt { get; protected set; }

    public DateTimeOffset? UpdatedAt { get; protected set; }

    public DateTimeOffset? DeletedAt { get; protected set; }

    public bool IsDeleted => DeletedAt.HasValue;

    protected void MarkAsUpdated(DateTimeOffset updatedAt)
    {
        if (IsDeleted)
        {
            throw new DomainException(
                "A deleted entity cannot be modified.");
        }

        if (updatedAt < CreatedAt)
        {
            throw new DomainException(
                "Update date cannot be earlier than creation date.");
        }

        UpdatedAt = updatedAt;
    }

    protected void MarkAsDeleted(DateTimeOffset deletedAt)
    {
        if (IsDeleted)
        {
            throw new DomainException(
                "The entity has already been deleted.");
        }

        if (deletedAt < CreatedAt)
        {
            throw new DomainException(
                "Delete date cannot be earlier than creation date.");
        }

        DeletedAt = deletedAt;
    }
}
