namespace Domain.Events;
public class ProductEditedEvent : BaseEvent
{
    public Product Product { get; }
    public ProductEditedEvent(Product product)
    {
        Product = product;
    }
}
