namespace Domain.Events;
public class ProductDeletedEvent : BaseEvent
{
    public Product Product { get; }
    public ProductDeletedEvent(Product product)
    {
        Product = product;
    }
}