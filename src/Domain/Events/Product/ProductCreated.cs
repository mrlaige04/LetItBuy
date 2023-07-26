namespace Domain.Events;
public class ProductCreated : BaseEvent
{
    public Product Product { get; init; }
    public ProductCreated(Product product)
    {
        Product = product;
    }
}