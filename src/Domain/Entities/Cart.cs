namespace Domain.Entities;
public class Cart : BaseAuditableEntity
{
    public ICollection<Product> Products { get; set; } = null!;
    public Cart()
    {
        Products = new List<Product>();
    }

    public Guid UserID { get; set; }
    public BaseUser User { get; set; } = null!;
}