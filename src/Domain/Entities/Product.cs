namespace Domain.Entities;
public class Product : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;

    public ICollection<ProductPhoto> Photos { get; set; } = null!;

    public Guid CategoryID { get; set; }
    public Category Category { get; set; } = null!;

    public Guid UserID { get; set; }
    public BaseUser User { get; set; } = null!;
}