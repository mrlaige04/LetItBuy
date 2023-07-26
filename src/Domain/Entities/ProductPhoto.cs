namespace Domain.Entities;
public class ProductPhoto : BaseEntity
{
    public string Base64Image { get; set; } = null!;

    public Guid ProductID { get; set; }
    public Product Product { get; set; } = null!;
}