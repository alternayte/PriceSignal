namespace Domain.Models.Waitlist;

public class Waitlist : BaseAuditableEntity
{
    public required string Email { get; init; }
}