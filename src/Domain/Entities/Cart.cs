namespace Domain.Entities;
public class Cart : BaseAuditableEntity
{
    public Guid UserID { get; set; }
    public BaseUser User { get; set; } = null!;
}