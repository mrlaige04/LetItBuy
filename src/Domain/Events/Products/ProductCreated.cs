namespace Domain.Events;
public class ProductCreated : BaseEvent
{
    public Product Product { get; }
    public ProductCreated(Product product)
    {
        Product = product;
    }
}