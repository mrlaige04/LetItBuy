namespace Domain.Events;
public class ProductDeleted : BaseEvent
{
    public Product Product { get; }
    public ProductDeleted(Product product)
    {
        Product = product;
    }
}