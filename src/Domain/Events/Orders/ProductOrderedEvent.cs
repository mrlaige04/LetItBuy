namespace Domain.Events;
public class ProductOrderedEvent : BaseEvent
{
    public Product Product { get; }
    public ProductOrderedEvent(Product product)
    {
        Product = product;
    }
}
