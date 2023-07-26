namespace Domain.Entities;
public class UserNotification : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;

    public Guid UserID { get; set; }
    public BaseUser User { get; set; } = null!;
}