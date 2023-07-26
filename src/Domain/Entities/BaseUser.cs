namespace Domain.Entities;
public abstract class BaseUser : BaseAuditableEntity
{
    public Cart Cart { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = null!;
}